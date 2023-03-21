using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Zumball.Core.Events.GamePlayEvents;

namespace Zumball.UI
{
    public class EndGameTransition : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(StartEndGameTransition());
        }

        private IEnumerator StartEndGameTransition()
        {
            Image img         = GetComponent<Image>();
            Color startColor  = new Color(0f, 0f, 0f, 0f);
            Color endColor    = new Color(0f, 0f, 0f, 1f);
            float elapsedTime = 0f;
            while (elapsedTime <= 1f)
            {
                elapsedTime += Time.deltaTime;
                img.color   =  Color.Lerp(startColor, endColor, elapsedTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            
            elapsedTime = 0f;
            while (elapsedTime <= 1f)
            {
                elapsedTime += Time.deltaTime;
                img.color   =  Color.Lerp(endColor, startColor, elapsedTime);
                yield return null;
            }
            
            gameObject.SetActive(false);
            
        }
    }
}