using UnityEngine;
using System.Collections;

namespace ARPGFX
{
    public class ARPGFXPortalScript : MonoBehaviour
    {
        [SerializeField]
        GameObject portalOpen;
        [SerializeField]
        GameObject portalIdle;
        [SerializeField]
        GameObject portalClose;


        void Start()
        {
            portalOpen.transform.position = transform.position;
            portalIdle.transform.position = transform.position;
            portalClose.transform.position = transform.position;
            portalOpen.transform.rotation = transform.rotation;
            portalIdle.transform.rotation = transform.rotation;
            portalClose.transform.rotation = transform.rotation;
            portalOpen.SetActive(false);
            portalIdle.SetActive(false);
            portalClose.SetActive(false);
        }

        public IEnumerator PortalEnterLoop()
        {
            while (true)
            {
                portalOpen.SetActive(true);

                yield return new WaitForSeconds(0.8f);

                portalIdle.SetActive(true);
                portalOpen.SetActive(false);
            }
        }

        public IEnumerator PortalExitLoop()
        {
            portalIdle.SetActive(false);
            portalClose.SetActive(true);

            yield return new WaitForSeconds(1f);

            portalClose.SetActive(false);
        }
    }
}