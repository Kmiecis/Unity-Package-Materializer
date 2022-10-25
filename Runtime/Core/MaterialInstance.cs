using UnityEngine;
using UnityEngine.UI;

namespace Common.Materializer
{
    public class MaterialInstance : MonoBehaviour
    {
        [SerializeField]
        private Material _original;
        [SerializeField]
        private int _depth;

#if UNITY_EDITOR
        private Material _cached;
#endif
        private Material _copy;

        public Material Original
        {
            get => _original;
            set
            {
                DestroyCopy();
                _original = value;
            }
        }

        public Material Copy
        {
            get
            {
                if (_copy is null && _original is not null)
                {
                    _copy = new Material(_original);
                    ApplyMaterial(_copy);
                }
                return _copy;
            }
        }

        public Material Current
        {
            get => _copy is null ? _original : _copy;
        }

        private void ApplyMaterial(Material material)
        {
            SetMaterial(material, transform, _depth);
        }

        private void DestroyCopy()
        {
            if (_copy is not null)
            {
                _copy.Destroy();
                _copy = null;
            }
        }

        private void Start()
        {
            ApplyMaterial(Current);
        }

        private void OnDestroy()
        {
            DestroyCopy();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!ReferenceEquals(_cached, _original))
            {
                DestroyCopy();

                _cached = _original;
            }

            ApplyMaterial(Current);
        }
#endif

        private static void SetMaterial(Material material, Transform target, int depth)
        {
            // Apply to UI
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                graphic.material = material;
            }

            // Apply to non-UI
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.sharedMaterial = material;
            }

            if (depth > 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialInstanceBlocker>(out _))
                    {
                        SetMaterial(material, child, depth - 1);
                    }
                }
            }
        }
    }
}
