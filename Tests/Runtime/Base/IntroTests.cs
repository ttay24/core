using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReactUnity.Helpers;
using ReactUnity.Scripting;
using UnityEngine;

namespace ReactUnity.Tests
{
    public class IntroTests : TestBase
    {
        public IntroTests(JavascriptEngineType engineType) : base(engineType) { }

        [ReactInjectableTest()]
        public IEnumerator EnsureCorrectEngineIsUsed()
        {
            yield return null;
            Assert.AreEqual(EngineType, Context.Script.EngineType);
        }

        [ReactInjectableTest(style: @"
            view { color: red; }
            view.blueClass { color: blue; }
            view.greenClass { color: magenta; }
            #test-id { color: white; }
")]
        public IEnumerator ClassListChangesCausesRerender()
        {
            var view = Q("view");

            var tmp = Canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            Assert.AreEqual("Hello world", tmp.text);
            Assert.AreEqual(Color.red, tmp.color);

            view.ClassList.Add("blueClass");
            yield return null;
            Assert.AreEqual(Color.blue, tmp.color);

            view.ClassName = "class-something another-class greenClass";
            yield return null;
            Assert.AreEqual(Color.magenta, tmp.color);

            view.Id = "test-id";
            yield return null;
            Assert.AreEqual(Color.white, tmp.color);
        }

        [ReactInjectableTest(@"
            Renderer.render(
                <view>
                    Hello world
                    <view>Hello again</view>
                    <view>
                        Somehow
                        <view> just hello</view>
                    </view>
                </view>
            );
        ")]
        public IEnumerator TextContent_IsCorrect()
        {
            yield return null;

            Assert.AreEqual("Hello worldHello againSomehow just hello", Host.TextContent);
        }

        [ReactInjectableTest(@"
            Renderer.render(<view>Hello world</view>);
            Renderer.render(<view>Hello world 2</view>);
        ")]
        public IEnumerator RerenderOfRootElementWorks()
        {
            yield return null;

            Assert.AreEqual("Hello world 2", Host.TextContent);
        }

        [ReactInjectableTest(@"
            function App() {
                const globals = ReactUnity.useGlobals();
                return <image active={globals.active} />;
            }

            Renderer.render(
                <GlobalsProvider>
                    <App />
                </GlobalsProvider>
            );
        ")]
        public IEnumerator ActivePropertyShouldSetGameObjectActive()
        {
            yield return null;

            var cmp = (Q("image") as UGUI.ImageComponent).GameObject;
            Assert.AreEqual(false, cmp.activeSelf);

            Component.Globals.Set("active", true);
            Assert.AreEqual(true, cmp.activeSelf);

            Component.Globals.Set("active", 0);
            Assert.AreEqual(false, cmp.activeSelf);
        }


        [ReactInjectableTest(@"
            function App() {
                const globals = ReactUnity.useGlobals();
                return <image source={globals.image} />;
            }

            Renderer.render(<App />);
        ")]
        public IEnumerator TestGlobalsChange()
        {
            yield return null;

            var imgCmp = (Host.QuerySelector("image") as UGUI.ImageComponent).Image;
            Assert.AreEqual(Texture2D.whiteTexture, imgCmp.mainTexture);

            var tx = new Texture2D(1, 1);
            Component.Globals.Set("image", tx);
            Assert.AreEqual(tx, imgCmp.mainTexture);
        }


        [ReactInjectableTest(@"
            function App() {
                const globals = ReactUnity.useGlobals();
                return <image source={globals.image} />;
            }

            Renderer.render(<App />);
        ")]
        public IEnumerator TestGlobalsChangeOnComponent()
        {
            yield return null;

            var imgCmp = (Host.QuerySelector("image") as UGUI.ImageComponent).Image;
            Assert.AreEqual(Texture2D.whiteTexture, imgCmp.mainTexture);

            var tx = new Texture2D(1, 1);
            Component.Globals["image"] = tx;
            Assert.AreEqual(tx, imgCmp.mainTexture);
        }

        [ReactInjectableTest(@"
            const watcher = ReactUnity.createDictionaryWatcher(Globals.inner, 'innerSerializable');
            function App() {
                const globals = watcher.useContext();
                return <image source={globals.image} />;
            }

            Renderer.render(
                <watcher.Provider>
                    <App />
                </watcher.Provider>
            );
        ", autoRender: false)]
        public IEnumerator TestArbitraryWatcher()
        {
            var sd = new SerializableDictionary();
            Globals["inner"] = sd;
            Render();
            yield return null;
            yield return null;

            var imgCmp = (Host.QuerySelector("image") as UGUI.ImageComponent).Image;
            Assert.AreEqual(Texture2D.whiteTexture, imgCmp.mainTexture);

            var tx = new Texture2D(1, 1);
            sd.Set("image", tx);
            Assert.AreEqual(tx, imgCmp.mainTexture);
        }

        [ReactInjectableTest]
        public IEnumerator HostNameCanBeChanged()
        {
            yield return null;
            Assert.AreEqual("REACT_ROOT", Host.GameObject.name);

            Host.Name = "hey";
            Assert.AreEqual("hey", Host.GameObject.name);

            Host.Name = null;
            Assert.AreEqual("REACT_ROOT", Host.GameObject.name);
        }

        [ReactInjectableTest]
        public IEnumerator ElementsAreRenderedInTheSameLayerAsHost()
        {
            yield return null;
            System.Func<Transform, IEnumerable<Transform>> selectAllChildren = null;
            selectAllChildren = (Transform x) => x.OfType<Transform>().SelectMany(x => selectAllChildren(x)).Concat(new List<Transform>() { x });
            var elements = selectAllChildren(Host.RectTransform);

            foreach (var item in elements)
            {
                Assert.AreEqual(Host.GameObject.layer, item.gameObject.layer);
            }
        }
    }
}
