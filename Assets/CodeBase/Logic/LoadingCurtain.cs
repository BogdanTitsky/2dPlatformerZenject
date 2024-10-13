using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        public CanvasGroup Curtain;

        private Coroutine _currentFadeCoroutine;

        public void Show()
        {
            if (_currentFadeCoroutine != null)
                StopCoroutine(_currentFadeCoroutine);

            gameObject.SetActive(true);
            Curtain.alpha = 1;
        }
    
        public void Hide()
        {
            if (_currentFadeCoroutine != null)
                StopCoroutine(_currentFadeCoroutine); 

            _currentFadeCoroutine = StartCoroutine(DoFadeIn());
        }

        private IEnumerator DoFadeIn()
        {
            while (Curtain.alpha > 0)
            {
                Curtain.alpha -= 0.03f;
                yield return new WaitForSeconds(0.03f);
            }
      
            gameObject.SetActive(false);
        }
    }
}