#if !(ENABLE_IL2CPP || REACT_DISABLE_CLEARSCRIPT || (UNITY_ANDROID && !UNITY_EDITOR)) && REACT_CLEARSCRIPT_AVAILABLE
#define REACT_CLEARSCRIPT
#endif

#if !REACT_DISABLE_QUICKJS && REACT_QUICKJS_AVAILABLE
#define REACT_QUICKJS
#endif

#if !REACT_DISABLE_JINT && REACT_JINT_AVAILABLE && (!UNITY_WEBGL || UNITY_EDITOR)
#define REACT_JINT
#endif

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReactUnity.Editor;
using ReactUnity.Editor.Renderer;
using ReactUnity.Editor.UIToolkit;
using ReactUnity.Helpers;
using ReactUnity.Scripting;
using ReactUnity.Styling;
using ReactUnity.Styling.Rules;
using ReactUnity.UIToolkit;
using UnityEditor;

namespace ReactUnity.Tests.Editor
{
#if REACT_JINT
    [TestFixture(JavascriptEngineType.Jint, Category = "Jint")]
#endif
#if REACT_CLEARSCRIPT
    [TestFixture(JavascriptEngineType.ClearScript, Category = "ClearScript")]
#endif
#if REACT_QUICKJS
    [TestFixture(JavascriptEngineType.QuickJS, Category = "QuickJS")]
#endif
    public abstract class EditorTestBase
    {
        protected TestReactWindow Window => EditorWindow.GetWindow<TestReactWindow>();
        protected ReactUnityEditorElement Component => Window?.HostElement;
        protected ReactContext Context => Component?.Context;
        protected EditorContext EditorContext => Context as EditorContext;
        protected IMediaProvider MediaProvider => Context?.MediaProvider;
        protected HostComponent Host => Context?.Host as HostComponent;
        protected GlobalRecord Globals => Context?.Globals;
        internal ReactUnityBridge Bridge => ReactUnityBridge.Instance;

        public readonly JavascriptEngineType EngineType;

        public EditorTestBase(JavascriptEngineType engineType)
        {
            EngineType = engineType;
        }

        public void Render() => Component.Run();
        public StyleSheet InsertStyle(string style, int importanceOffset = 0) => Context.InsertStyle(style, importanceOffset);
        public void RemoveStyle(StyleSheet sheet) => Context.RemoveStyle(sheet);
        public IUIToolkitComponent Q(string query, IReactComponent scope = null) =>
            (scope ?? Host).QuerySelector(query) as IUIToolkitComponent;
        public List<BaseReactComponent<UIToolkitContext>> QA(string query, IReactComponent scope = null) =>
            (scope ?? Host).QuerySelectorAll(query).OfType<BaseReactComponent<UIToolkitContext>>().ToList();
        public IEnumerator AdvanceTime(float advanceBy)
        {
            if (Context.Timer is EditorTimer)
            {
                yield return new EditModeWaitForSeconds(advanceBy).Perform();
            }
            else
            {
                yield return Context.Timer.Yield(advanceBy);
            }
        }

        [OneTimeSetUp]
        [OneTimeTearDown]
        public void TearDownFixture()
        {
            if (EditorWindow.HasOpenInstances<TestReactWindow>())
                if (Window != null) Window.Close();
        }
    }
}
