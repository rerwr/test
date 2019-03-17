using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using Game;


namespace Framework
{
    /// <summary>
    /// MultiThreadRunner
    /// 以类似Unity协程的方式写多线程，并可以方便地yield return回主线程以运行下一段。
    /// 如yield return 此类中的TYPE_*成员，效果如成员所示(不用枚举类避免装箱）
    /// 也可yield return float,则会延时此浮点数所示的时间并在Update函数中运行下一段
    /// 请不要yield return 其它类型
    /// </summary>
    public class MTRunner : SingletonMonoBehaviour<MTRunner>
    {
        private Thread _thread;
        private volatile float _time;
        private volatile bool _active;
        private const int KEY_UPDATE = 0;
        private const int KEY_FIXED = 1;
        private const int KEY_LATE = 2;
        private const int KEY_ANY = 3;
        private const int KEY_ASYN = 4;
        /// <summary>
        /// 以下在Update阶段运行
        /// </summary>
        public static readonly object TYPE_Upate = new YieldType(KEY_UPDATE);
        /// <summary>
        /// 以下在FixedUpdate阶段运行
        /// </summary>
        public static readonly object TYPE_Fixed = new YieldType(KEY_FIXED);
        /// <summary>
        /// 以下在LateUpdate阶段运行
        /// </summary>
        public static readonly object TYPE_Late = new YieldType(KEY_LATE);
        /// <summary>
        /// 以下在任意主线程阶段运行，如果你急的话用这个
        /// </summary>
        public static readonly object TYPE_AnyMain = new YieldType(KEY_ANY);
        /// <summary>
        /// 以下从Runtime线程池中取线程运行，注意别在其它线程调用Unity的某些函数，不然会出错
        /// </summary>
        public static readonly object TYPE_Asyn = new YieldType(KEY_ASYN);

        private readonly Queue<IEnumerator> _updateQueue = new Queue<IEnumerator>();
        private readonly Queue<IEnumerator> _fixedQueue = new Queue<IEnumerator>();
        private readonly Queue<IEnumerator> _lateQueue = new Queue<IEnumerator>();
        private readonly Queue<IEnumerator> _anyMainQueue = new Queue<IEnumerator>();
        private readonly Queue<IEnumerator> _asynQueue = new Queue<IEnumerator>();
        /// <summary>
        /// 为防止无限循环，runner运行结束又想加回当前队列的，先加到临时队列，以推迟到下一帧,这也是调用者yield的意愿。
        /// asy类runner无此问题（因为不在分派线程运行），故这个队列只在主线程操作，不必加锁
        /// </summary>
        private readonly Queue<IEnumerator> _tempQueue = new Queue<IEnumerator>();
        /// <summary>
        /// 定时队列，这里保存下次运行的时间点
        /// </summary>
        private readonly List<float> _timer = new List<float>();
        /// <summary>
        /// 保存定时队列的运行器
        /// </summary>
        private readonly List<IEnumerator> _timeRunner = new List<IEnumerator>();
        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            _active = true;
            _thread = new Thread(Run);
            _thread.Start();
        }

        void Start()
        {
            _time = Time.time;
        }

        /// <summary>
        /// 开始运行，调用方法类似协程
        /// 注意在第一个yield return之前的代码会在当前线程立即执行
        /// </summary>
        /// <param name="runner"></param>
        public void StartRunner(IEnumerator runner)
        {
            if (runner.MoveNext())
            {
                DispatchYield(runner, runner.Current);
            }
        }

        public void RunOnMainThread(Action _action)
        {
            StartRunner(RunnerHelper(_action));
        }

        IEnumerator RunnerHelper(Action _action)
        {
            yield return TYPE_Upate;
            _action();
        }

        void Update()
        {
            RunQueue(_updateQueue, TYPE_Upate);
            RunQueue(_anyMainQueue, TYPE_AnyMain);
            RunTimer();
        }

        private void RunTimer()
        {
            _time = Time.time;
            while (_timer.Count > 0)
            {
                IEnumerator runner = null;
                lock (_timer)
                {
                    int i = _timer.Count - 1;
                    float nexttime = _timer[i];
                    if (nexttime > _time)
                    {
                        break;
                    }
                    runner = _timeRunner[i];
                    _timer.RemoveAt(i);
                    _timeRunner.RemoveAt(i);
                }
                try
                {
                    if (runner != null && runner.MoveNext())
                    {
                        DispatchYield(runner, runner.Current);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + "\n" + e.StackTrace);
                }
            }
        }

        void LateUpdate()
        {
            RunQueue(_lateQueue, TYPE_Late);
            RunQueue(_anyMainQueue, TYPE_AnyMain);
        }

        void FixedUpdate()
        {
            RunQueue(_fixedQueue, TYPE_Fixed);
            RunQueue(_anyMainQueue, TYPE_AnyMain);
        }

        void OnDisable()
        {
            _active = false;
            lock (_updateQueue)
            {
                _updateQueue.Clear();
            }
            lock (_fixedQueue)
            {
                _fixedQueue.Clear();
            }
            lock (_lateQueue)
            {
                _lateQueue.Clear();
            }
            lock (_anyMainQueue)
            {
                _anyMainQueue.Clear();
            }
            lock (_asynQueue)
            {
                _asynQueue.Clear();
            }
            _tempQueue.Clear();
        }

        void RunQueue(Queue<IEnumerator> queue, object type)
        {
            if (!_active)
                return;
            while (queue.Count > 0)
            {
                IEnumerator runner = null;
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        runner = queue.Dequeue();
                    }
                }
                if (runner != null)
                {
                    try
                    {
                        if (runner.MoveNext())
                        {
                            object yieldresult = runner.Current;
                            if (yieldresult == type)
                            {
                                _tempQueue.Enqueue(runner);
                            }
                            else
                            {
                                DispatchYield(runner, runner.Current);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message + "\n" + e.StackTrace);
                    }
                }
            }
            while (_tempQueue.Count > 0)
            {
                IEnumerator runner = _tempQueue.Dequeue();
                DispatchYield(runner, runner.Current);
            }
        }
        void Run()
        {
            while (_active)
            {
                Thread.Sleep(1);
                while (_asynQueue.Count > 0)
                {
                    IEnumerator runner = null;
                    lock (_asynQueue)
                    {
                        if (_asynQueue.Count > 0)
                        {
                            runner = _asynQueue.Dequeue();
                        }
                    }
                    if (runner != null)
                    {
                        ThreadPool.QueueUserWorkItem(RunnerWrap, runner);
                    }
                }
            }
        }

        void RunnerWrap(object ienum)
        {
            IEnumerator runner = (IEnumerator)ienum;
            try
            {
                if (runner.MoveNext())
                {
                    DispatchYield(runner, runner.Current);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
            }

        }

        void DispatchYield(IEnumerator runner, object yieldResult)
        {
            if (!_active)
                return;
            if (yieldResult is YieldType)
            {
                YieldType y = (YieldType)yieldResult;
                switch (y.id)
                {
                    case KEY_UPDATE:
                        AddTo(runner, _updateQueue);
                        break;
                    case KEY_FIXED:
                        AddTo(runner, _fixedQueue);
                        break;
                    case KEY_LATE:
                        AddTo(runner, _lateQueue);
                        break;
                    case KEY_ANY:
                        AddTo(runner, _anyMainQueue);
                        break;
                    case KEY_ASYN:
                        AddTo(runner, _asynQueue);
                        break;
                    default:
                        Debug.LogError("yield result id error=====>" + y.id);
                        break;
                }
            }
            else if(yieldResult is float|| yieldResult is int)
            {
                float nextTime = (float)yieldResult + _time;
                lock (_timer)
                {
                    int i = _timer.Count - 1;
                    while (i >= 0)
                    {
                        if (nextTime < _timer[i])
                        {
                            break;
                        }
                        i--;
                    }
                    i++;
                    if (i == _timer.Count)
                    {
                        _timer.Add(nextTime);
                        _timeRunner.Add(runner);
                    }
                    else
                    {
                        _timer.Insert(i, nextTime);
                        _timeRunner.Insert(i, runner);
                    }
                }
            }
            else 
            {
                Debug.LogError("yield result id error=====>" + yieldResult.GetType().FullName + ":" + yieldResult.ToString());
            }
        }

        private void AddTo(IEnumerator runner, Queue<IEnumerator> queue)
        {
            lock (queue)
            {
                queue.Enqueue(runner);
            }
        }

        private class YieldType
        {
            public readonly int id;
            public YieldType(int id)
            {
                this.id = id;
            }
        }
    }
}

