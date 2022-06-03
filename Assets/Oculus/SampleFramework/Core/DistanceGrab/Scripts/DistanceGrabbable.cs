/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using UnityEngine;

namespace OculusSampleFramework
{
    public class DistanceGrabbable : OVRGrabbable
    {
        public string m_materialColorField;

        GrabbableCrosshair m_crosshair;
        protected GrabManager m_crosshairManager;
        [SerializeField] protected Renderer m_renderer;
        protected MaterialPropertyBlock m_mpb;


        public virtual bool InRange
        {
            get { return m_inRange; }
            set
            {
                m_inRange = value;
                RefreshCrosshair();
            }
        }
        protected bool m_inRange;

        public virtual bool Targeted
        {
            get { return m_targeted; }
            set
            {
                m_targeted = value;
                RefreshCrosshair();
            }
        }
        protected bool m_targeted;

        protected override void Start()
        {
            base.Start();
            m_crosshair = gameObject.GetComponentInChildren<GrabbableCrosshair>();

            if (m_renderer == null)
                m_renderer = gameObject.GetComponent<Renderer>();

            m_crosshairManager = FindObjectOfType<GrabManager>();
            m_mpb = new MaterialPropertyBlock();
            RefreshCrosshair();

            if (m_renderer != null)
                m_renderer.SetPropertyBlock(m_mpb);
        }

        void RefreshCrosshair()
        {
            if (m_crosshair)
            {
                if (isGrabbed) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else if (!InRange) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else m_crosshair.SetState(Targeted ? GrabbableCrosshair.CrosshairState.Targeted : GrabbableCrosshair.CrosshairState.Enabled);
            }
            if (m_materialColorField != null)
            {
                if (m_renderer != null)
                    m_renderer.GetPropertyBlock(m_mpb);

                if (isGrabbed || !InRange) m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorOutOfRange);
                else if (Targeted) m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorHighlighted);
                else m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorInRange);

                if (m_renderer != null)
                    m_renderer.SetPropertyBlock(m_mpb);
            }
        }
    }
}
