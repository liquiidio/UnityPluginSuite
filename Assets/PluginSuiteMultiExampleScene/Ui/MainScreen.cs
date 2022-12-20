using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    /*
     * Child-Controls
     */
    private VisualElement _menu;
    private VisualElement _maskBox;
    private VisualElement _canvasWidgetScene;
    private VisualElement _uiTookitWidgetScene;
    private VisualElement[] _mainMenuOptions;
    private List<VisualElement> _widgets;

    private Label _anchorLabel;
    private Label _ualLabel;
    private Label _atomicAssetLabel;
    private Label _atomicMarketLabel;
    private Label _hyperionLabel;
    private Label _quitLabel;

    private Button _closeViewButton;

    public VisualElement Root;

    /*
     * Fields/Properties
     */
    private const string PopUpAnimation = "pop-animation-hide";

    private string _currentSceneLoaded = "";
    private string _clickedButtonName = string.Empty;

    [SerializeField] internal StyleSheet MainScreenAnimatedStyleSheet;
    [SerializeField] internal StyleSheet MainScreenStyleSheet;

    void Start()
    {
         Root = GetComponent<UIDocument>().rootVisualElement;

        _menu = Root.Q<VisualElement>("menu");
        _maskBox = Root.Q<VisualElement>("mask-box");
        _maskBox.style.visibility = Visibility.Hidden;
        _maskBox.style.display = DisplayStyle.None;

        _mainMenuOptions = _menu.Q<VisualElement>("main-nav").Children().ToArray();
        _widgets = Root.Q<VisualElement>("body").Children().ToList();
        _canvasWidgetScene = Root.Q<VisualElement>("canvas-widget-container");
        _uiTookitWidgetScene = Root.Q<VisualElement>("ui-toolkit-widget-container");

        _anchorLabel = Root.Q<Label>("anchor-label");
        _ualLabel = Root.Q<Label>("ual-label");
        _atomicAssetLabel = Root.Q<Label>("atomic-asset-label");
        _atomicMarketLabel = Root.Q<Label>("atomic-market-label");
        _hyperionLabel = Root.Q<Label>("hyperion-label");
        _quitLabel = Root.Q<Label>("quit-label");

        _closeViewButton = Root.Q<Button>("close-view-button");

        StartCoroutine(PopupMenuAnimation());
        BindButtons();
        CheckStylesheet();
    }

    #region Button Binding

    private void BindButtons()
    {

#if UNITY_2021_0_OR_NEWER
#else
#endif


#if UNITY_WEBGL
        _quitLabel.style.visibility = Visibility.Hidden;
        _quitLabel.style.display = DisplayStyle.None;
#endif


#if UNITY_2021_0_OR_NEWER
        _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 254, 0)));
#else
        _widgets.ForEach(x => x.style.visibility = Visibility.Hidden);
#endif

        _closeViewButton.clickable.clicked += () =>
        {
            if (_currentSceneLoaded != "")
            {
                SceneManager.UnloadSceneAsync(_currentSceneLoaded);
                _currentSceneLoaded = "";

                _maskBox.style.visibility = Visibility.Hidden;
                _maskBox.style.display = DisplayStyle.None;


#if UNITY_2021_0_OR_NEWER
                _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 254, 0)));
#else
                _widgets.ForEach(x => x.style.visibility = Visibility.Hidden);
#endif
            }
        };

        _anchorLabel.RegisterCallback<ClickEvent>(evt =>
        {
            _clickedButtonName = _anchorLabel.text;

#if UNITY_2021_0_OR_NEWER
            _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 0, 0)));
#else
            _widgets.ForEach(x => x.style.visibility = Visibility.Visible);
#endif
        });

        _ualLabel.RegisterCallback<ClickEvent>(evt =>
        {
            _clickedButtonName = _ualLabel.text;

#if UNITY_2021_0_OR_NEWER
            _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 0, 0)));
#else
            _widgets.ForEach(x => x.style.visibility = Visibility.Visible);
#endif
        });

        _atomicAssetLabel.RegisterCallback<ClickEvent>(evt =>
        {
            LoadScene("AtomicAssetsExampleScene");
        });

        _atomicMarketLabel.RegisterCallback<ClickEvent>(evt =>
        {
            LoadScene("AtomicMarketExampleScene");
        });

        _hyperionLabel.RegisterCallback<ClickEvent>(evt =>
        {
            LoadScene("HyperionExampleScene");
        });

        _quitLabel.RegisterCallback<ClickEvent>(evt =>
        {
            Application.Quit();
        });

        _canvasWidgetScene.RegisterCallback<ClickEvent>(evt =>
        {
            switch (_clickedButtonName)
            {
                case "ANCHOR":
                    LoadScene("ExampleCanvasScene");
                    break;
                case "UAL":
                    LoadScene("CanvasScene");
                    break;
            }
        });

        _uiTookitWidgetScene.RegisterCallback<ClickEvent>(evt =>
        {
            switch (_clickedButtonName)
            {
                case "ANCHOR":
                    LoadScene("UiToolkitAnchorExampleScene");
                    break;
                case "UAL":
                    LoadScene("UiToolkitUALExampleScene");
                    break;
                case "ATOMIC ASSETS":
                    LoadScene("AtomicAssetsExampleScene");
                    break;
                case "ATOMIC MARKET":
                    LoadScene("AtomicMarketExampleScene");
                    break;
                case "HYPERION":
                    LoadScene("HyperionExampleScene");
                    break;
            }
        });
    }

    #endregion

    #region Others

    private IEnumerator PopupMenuAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        _menu.ToggleInClassList(PopUpAnimation);
    }

    private void LoadScene(string targetScene)
    {
        _maskBox.style.visibility = Visibility.Visible;
        _maskBox.style.display = DisplayStyle.Flex;

        _currentSceneLoaded = targetScene;
        SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }



    // check which stylesheet to use (animated or without animation)
    private void CheckStylesheet()
    {
        //Root.styleSheets.Clear();

#if UNITY_2021_0_OR_NEWER
        Root.styleSheets.Remove(MainScreenStyleSheet);
        Root.styleSheets.Add(MainScreenAnimatedStyleSheet);
#else
        //Root.styleSheets.Remove(MainScreenAnimatedStyleSheet);
        //Root.styleSheets.Add(MainScreenStyleSheet);

        //Root.styleSheets.Remove(MainScreenStyleSheet);
        //Root.styleSheets.Add(MainScreenStyleSheet);
#endif
    }

    #endregion
}


