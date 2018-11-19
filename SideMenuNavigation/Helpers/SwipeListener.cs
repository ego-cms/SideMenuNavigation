using System;
using Android.Views;

namespace SideMenuNavigation.Helpers
{
    public class SwipeListener
    {
        private const float MinScrollWidth = 20;

        private float _bufferScrollX;
        private float _bufferScrollY;
        private bool _isScrollMovedRight;
        private bool _isScrollMovedLeft;

        /// <summary>
        /// Detect is touch event horisontal swipe
        /// </summary>
        /// <param name="isForward">Ref is swipe left to right or right to left</param>
        /// <param name="motionEvent">Motionevent of view</param>
        /// <returns>If current event was horizontal swipe and it's swipe more then min count of pixels</returns>
        public bool IsSwipedHorizontal(ref bool isForward, MotionEvent motionEvent)
        {
            _isScrollMovedRight = false;
            _isScrollMovedLeft = false;

            if(motionEvent.Action == MotionEventActions.Down)
            {
                _bufferScrollX = motionEvent.RawX;
                _bufferScrollY = motionEvent.RawY;
                return false;
            }
            else if(motionEvent.Action == MotionEventActions.Move && _bufferScrollX != motionEvent.RawX && (!_isScrollMovedLeft && !_isScrollMovedRight))
            {
                if(_bufferScrollX != 0 && _bufferScrollX - motionEvent.RawX > MinScrollWidth && Math.Abs(_bufferScrollX - motionEvent.RawX) > Math.Abs(_bufferScrollY - motionEvent.RawY))
                    _isScrollMovedLeft = true;

                else if(_bufferScrollX != 0 && _bufferScrollX - motionEvent.RawX < -MinScrollWidth && Math.Abs(_bufferScrollX - motionEvent.RawX) > Math.Abs(_bufferScrollY - motionEvent.RawY))
                    _isScrollMovedRight = true;

                if(_isScrollMovedRight || _isScrollMovedLeft)
                {
                    isForward = _isScrollMovedRight;
                    _bufferScrollX = 0f;
                    _bufferScrollY = 0f;
                }

                return _isScrollMovedRight || _isScrollMovedLeft;
            }
            return false;
        }

    }
}