

using System.Collections.Generic;
using Framework;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    using System;
    using System.Runtime.CompilerServices;


    using UnityEngine;

    public sealed class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private bool _clickCancelled;
        private bool _clickPrevented;
        private WorldObject _currentHitTarget;
        private float _currentPinchDistance;
        private WorldObject _dragObject;

        private bool _isEscapeDown;
        private bool _isMouseDown;
        private bool _isMouseMoved;
        private Vector2 _mousePosition;
        private bool _mouseWheelEnabled;
        private bool _pinchStarted;
        private float _startPinchDistance;
        public TouchEventArgs _touchArgs = new TouchEventArgs();
        private int _touchCount;
        private float _wheelDelta;
        private const float MoveTreshold = 0.1f;

        public event Action Escape;

        public event Action MouseWheel;

        public event Action PinchDistanceChanged;

        public event Action PinchEnded;

        public event Action PinchStarted;
        private Canvas c;
        void Start()
        {
            try
            {
                _mousePosition = Input.mousePosition;
                this._wheelDelta = Input.GetAxis("Mouse ScrollWheel");
                this._mouseWheelEnabled = true;
                c = GameObject.Find("Canvas").GetComponent<Canvas>();
            }
            catch
            {
                this._mouseWheelEnabled = false;
            }
        }

        public void CancelNextClick()
        {
            this._clickCancelled = true;
        }

        public void PreventClick()
        {
            this._clickPrevented = true;
        }

        public void Update()
        {


            this.UpdateMouseWheel();
            bool mouseDown = false;
            bool StartClick = false;
            bool ClickEnded = false;
            this._touchCount = Input.touchCount;
            Vector2 vector = this._mousePosition;
            if (this._touchCount < 2)
            {
                this._mousePosition = Input.mousePosition;
            }
            else
            {
                this._mousePosition = Input.GetTouch(0).position;
            }

            Vector2 vector2 = this._mousePosition - vector;
            //是否移动
            this._isMouseMoved = vector2.magnitude >= 0.1f;
            //两个手指取消点击
            if (this._touchCount > 1)
            {
                this._clickCancelled = true;
            }
            //一个手指点击
            if ((this._touchCount < 2) && Input.GetMouseButton(0))
            {
                mouseDown = true;
            }

            if (this._isMouseDown && !mouseDown)
            {
                //松开点击
                ClickEnded = true;
            }
            else if (!this._isMouseDown && mouseDown)
            {
                //开始点击
                StartClick = true;
            }
            //退出键点击
            if (Input.GetKeyDown(KeyCode.Escape) && !this._isEscapeDown)
            {
                this._isEscapeDown = true;
                if (this.Escape != null)
                {
                    this.Escape();
                }
            }

            //            Debug.Log("------test------");
            //退出点击完成
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                this._isEscapeDown = false;
            }
            this._isMouseDown = mouseDown;
            //两只手指的情况，以及pc端模拟
            if ((this._touchCount == 2) || Input.GetKey(KeyCode.LeftShift))
            {
                //左shift模拟
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (!this._pinchStarted)
                    {
                        this._currentPinchDistance = 100f;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        this._currentPinchDistance += 10f;
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        this._currentPinchDistance -= 10f;
                    }
                }
                else
                {
                    //两手指捏的距离
                    Vector2 vector3 = this._mousePosition - Input.GetTouch(1).position;
                    this._currentPinchDistance = vector3.magnitude;
                }
                if (this._pinchStarted)
                {
                    if (this.PinchDistanceChanged != null)
                    {
                        //手势距离发生变化
                        this.PinchDistanceChanged();

                    }
                }
                else
                {
                    //开始手势
                    this._pinchStarted = true;
                    this._startPinchDistance = this._currentPinchDistance;
                    if (this.PinchStarted != null)
                    {
                        this.PinchStarted();
                    }
                }
            }
            else if (this._pinchStarted)
            {
                //如果不用两只手指，并且已经开始过，表示手势结束
                this._pinchStarted = false;
                if (this.PinchEnded != null)
                {
                    this.PinchEnded();
                }
                this._startPinchDistance = this._currentPinchDistance = 0f;
            }
            //取消点击
            this._touchArgs.Cancelled = this._clickCancelled || this._clickPrevented;
            this._clickPrevented = false;
            if (StartClick)
            {
                this._touchArgs.Phase = TouchPhase.Began;

            }
            else if (ClickEnded)
            {
                this._touchArgs.Phase = TouchPhase.Ended;
                this._clickCancelled = false;
            }
            else if (this._isMouseMoved && this._isMouseDown)
            {
                this._touchArgs.Phase = TouchPhase.Moved;
            }
            else
            {
                //手指点击停留
                this._touchArgs.Phase = TouchPhase.Stationary;
            }

            if (this._dragObject != null)
            {
                //拖动物体赋值
                this._touchArgs.Target = this._dragObject;
            }
            else if (this._touchArgs.Phase != TouchPhase.Stationary)
            {

                if (!IsPointerOverUIObject(c, Input.mousePosition))

                {
                    Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition); //从摄像机发出到点击坐标的射线
                    Vector2 vec2 = new Vector2(vec3.x, vec3.y);
                    RaycastHit2D hitInfo = Physics2D.Raycast(vec2, Vector2.zero);

                    if (hitInfo.collider != null)
                    {
//                        Debug.Log(hitInfo.collider.name);
                        //如果点击到背景
                        if (hitInfo.collider.name.Contains("BG"))
                        {
                            //复位
                            if (ViewMgr.Instance.isOpen(ViewNames.FriendsListView))
                            {
                                 ViewMgr.Instance.Close(ViewNames.FriendsListView);

                            }
                            SeedBarView.PlayBack();
                            SeedActionModel.currentId_Seed = 0;
                            CommonActionBarView.Action1 = GameAction.None;
                            if (ViewMgr.Instance.isOpen(ViewNames.TimeBarView))
                            {
                                ViewMgr.Instance.Close(ViewNames.TimeBarView);
                           
                                //(ViewMgr.Instance.views[ViewNames.CommonView].Viewlist[0]
                                //   .subViews[2] as CommonPlayerInfoView).isClicked = false ;
                            }

                        }
                        if (hitInfo.collider.name.Contains("land"))
                        {
                            Dictionary<int, FarmUnit> farms = FieldsModel.Instance.farms; ;

                            int id = int.Parse(hitInfo.collider.name.Substring(4));


                            WorldObject farmUnit = farms[id];
                            _touchArgs.Target = farmUnit;
                        }                       
                        else
                        {

                            Dictionary<string, WorldObject> obj = FieldsModel.Instance.otherObjs;

                            if (obj.ContainsKey(hitInfo.collider.name))
                            {

                                Debug.Log(hitInfo.collider.name);
                                _touchArgs.Target = obj[hitInfo.collider.name];
                            }
                        }
                    }
                }



                if (this._touchArgs.Target != this._currentHitTarget)
                {
                    if (this._currentHitTarget != null)
                    {
                        //世界物体点击入口
                        this._currentHitTarget.DispatchTouch(this._touchArgs);

                    }
                    this._currentHitTarget = this._touchArgs.Target;
                }
                if (this._currentHitTarget != null)
                {
                    this._currentHitTarget.DispatchTouch(this._touchArgs);
                }
                if (ClickEnded)
                {
                    this._currentHitTarget = null;
                    this._touchArgs.Target = null;
                }
            }
        }

        private void UpdateMouseWheel()
        {
            if (this._mouseWheelEnabled)
            {
                this._wheelDelta = Input.GetAxis("Mouse ScrollWheel");
                if (Math.Abs(this._wheelDelta) > float.Epsilon)
                {
                    this._wheelDelta = (this._wheelDelta <= 0f) ? -1f : 1f;
                    if (this.MouseWheel != null)
                    {
                        this.MouseWheel();
                    }
                }
            }
        }

        public float CurrentPinchDistance
        {
            get
            {
                return this._currentPinchDistance;
            }
        }

        public WorldObject DragObject
        {
            get
            {
                return this._dragObject;
            }
            set
            {
                this._dragObject = value;
            }
        }


        public Vector2 MousePosition
        {
            get
            {
                return this._mousePosition;
            }
        }

        public float StartPinchDistance
        {
            get
            {
                return this._startPinchDistance;
            }
        }

        public float WheelDelta
        {
            get
            {
                return this._wheelDelta;
            }
        }

        private static bool IsPointerOverUIObject()
        {
            if (EventSystem.current == null)
                return false;

            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
        


        /// <summary>
        /// Cast a ray to test if screenPosition is over any UI object in canvas. This is a replacement
        /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
        /// </summary>
        private bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
        {
            if (EventSystem.current == null)
                return false;

            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}

