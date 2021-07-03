using ReactUnity.Editor.UIToolkit;
using ReactUnity.Schedulers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using ReactUnity.StyleEngine;
using ReactUnity.Helpers;
using ReactUnity.Dispatchers;
using ReactUnity.UIToolkit;

namespace ReactUnity.Editor.Renderer
{
    public class EditorContext : UIToolkitContext
    {
        public static new Dictionary<string, Func<string, string, EditorContext, IUIToolkitComponent<VisualElement>>> ComponentCreators
            = new Dictionary<string, Func<string, string, EditorContext, IUIToolkitComponent<VisualElement>>>()
            {
                { "color", (tag, text, context) => new BaseFieldComponent<ColorField, Color>(context, tag) },
                { "bounds", (tag, text, context) => new BaseFieldComponent<BoundsField, Bounds>(context, tag)},
                { "boundsint", (tag, text, context) => new BaseFieldComponent<BoundsIntField, BoundsInt>(context, tag)},
                { "curve", (tag, text, context) => new BaseFieldComponent<CurveField, AnimationCurve>(context, tag) },
                { "double", (tag, text, context) => new BaseFieldComponent<DoubleField, double>(context, tag) },
                { "enum", (tag, text, context) => new EnumComponent<EnumField>(context, tag) },
                { "flags", (tag, text, context) => new EnumComponent<EnumFlagsField>(context, tag)},
                { "float", (tag, text, context) => new BaseFieldComponent<FloatField, float>(context, tag) },
                { "gradient", (tag, text, context) => new BaseFieldComponent<GradientField, Gradient>(context, tag) },
                { "inspector", (tag, text, context) => new BindableComponent<InspectorElement>(context, tag) },
                { "integer", (tag, text, context) => new BaseFieldComponent<IntegerField, int>(context, tag) },
                { "layer", (tag, text, context) => new BaseFieldComponent<LayerField, int>(context, tag) },
                { "layermask", (tag, text, context) => new BaseFieldComponent<LayerMaskField, int>(context, tag) },
                { "long", (tag, text, context) => new BaseFieldComponent<LongField, long>(context, tag) },
                { "mask", (tag, text, context) => new BaseFieldComponent<MaskField, int>(context, tag) },
                { "object", (tag, text, context) => new ObjectComponent(context) },
                { "property", (tag, text, context) => new BindableComponent<PropertyField>(context, tag) },
                { "rect", (tag, text, context) => new BaseFieldComponent<RectField, Rect>(context, tag) },
                { "rectint", (tag, text, context) => new BaseFieldComponent<RectIntField, RectInt>(context, tag) },
                { "tag", (tag, text, context) => new BaseFieldComponent<TagField, string>(context, tag) },
                { "vector2", (tag, text, context) => new BaseFieldComponent<Vector2Field, Vector2>(context, tag) },
                { "vector2int", (tag, text, context) => new BaseFieldComponent<Vector2IntField, Vector2Int>(context, tag) },
                { "vector3", (tag, text, context) => new BaseFieldComponent<Vector3Field, Vector3>(context, tag) },
                { "vector3int", (tag, text, context) => new BaseFieldComponent<Vector3IntField, Vector3Int>(context, tag) },
                { "vector4", (tag, text, context) => new BaseFieldComponent<Vector4Field, Vector4>(context, tag) },
                { "length", (tag, text, context) => new BaseFieldComponent<StyleLengthField, StyleLength>(context, tag) },
                { "toolbar", (tag, text, context) => new UIToolkitComponent<Toolbar>(context, tag) },
                { "tb-breadcrumbs", (tag, text, context) => new UIToolkitComponent<ToolbarBreadcrumbs>(context, tag) },
                { "tb-button", (tag, text, context) => new ButtonComponent<ToolbarButton>(context, tag) },
                { "tb-menu", (tag, text, context) => new UIToolkitComponent<ToolbarMenu>(context, tag) }, // TODO
                { "tb-popupsearch", (tag, text, context) => new UIToolkitComponent<ToolbarPopupSearchField>(context, tag) }, // TODO
                { "tb-search", (tag, text, context) => new UIToolkitComponent<ToolbarSearchField>(context, tag) }, // TODO
                { "tb-spacer", (tag, text, context) => new UIToolkitComponent<ToolbarSpacer>(context, tag) },
                { "tb-toggle", (tag, text, context) => new ToggleComponent<ToolbarToggle>(context, tag) },
            };

        public EditorContext(VisualElement hostElement, GlobalRecord globals, ReactScript script, IDispatcher dispatcher,
            IUnityScheduler scheduler, IMediaProvider mediaProvider, bool isDevServer, Action onRestart = null)
            : base(hostElement, globals, script, dispatcher, scheduler, mediaProvider, isDevServer, onRestart)
        {
        }

        public override IReactComponent CreateComponent(string tag, string text)
        {
            if (!ComponentCreators.TryGetValue(tag, out var creator)) return base.CreateComponent(tag, text);

            IUIToolkitComponent<VisualElement> res = creator(tag, text, this);
            if (res.Element != null) res.Element.name = $"<{tag}>";
            return res;
        }

        public override void PlayAudio(AudioClip clip)
        {
            EditorSFX.PlayClip(clip);
        }
    }
}
