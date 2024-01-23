using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    public abstract class MaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances = null;

        [SerializeField] protected T _value;

        public T Value
        {
            get => _value;
            set => ApplyPropertyValue(value);
        }

        protected abstract void ApplyPropertyValue(Material material, T value);

        protected abstract T ReadPropertyValue(Material material);

        private IEnumerable<Material> GetMaterialCopies()
        {
            foreach (var instance in _instances)
            {
                yield return instance.Copy;
            }
        }

        private void ApplyPropertyValue()
        {
            ApplyPropertyValue(_value);
        }

        private void ApplyPropertyValue(T value)
        {
            foreach (var material in GetMaterialCopies())
            {
                ApplyPropertyValue(material, value);
            }

            _value = value;
        }

        private void Start()
        {
            ApplyPropertyValue();
        }

        private void OnDidApplyAnimationProperties()
        {
            ApplyPropertyValue();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            ApplyPropertyValue();
        }

        protected virtual void Reset()
        {
            _instances = transform.GetComponentsInChildren<MaterialInstance>();
        }
#endif
    }
}
