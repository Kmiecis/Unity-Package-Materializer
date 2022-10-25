using UnityEngine;

namespace Common.Materializer
{
    public class MaterialFloat : AMaterialValue<float>
    {
        protected override void ApplyPropertyValue(Material material, int id, float value)
        {
            material.SetFloat(id, value);
        }

        protected override float ReadPropertyValue(Material material, int id)
        {
            return material.GetFloat(id);
        }
    }
}
