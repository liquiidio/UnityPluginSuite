using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;
using Assets.Packages.AnchorLinkTransportSharp.Src.Transports.UiToolkit.Ui;
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

    /*
     * Fields/Properties
     */
    private const string PopUpAnimation = "pop-animation-hide";

    private string _currentSceneLoaded = "";
    private int _mainPopupIndex = -1;
    private string _clickedButtonName = string.Empty;


    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _menu = root.Q<VisualElement>("menu");
        _maskBox = root.Q<VisualElement>("mask-box");
        _maskBox.style.visibility = Visibility.Hidden;

        _mainMenuOptions = _menu.Q<VisualElement>("main-nav").Children().ToArray();
        _widgets = root.Q<VisualElement>("body").Children().ToList();
        _canvasWidgetScene = root.Q<VisualElement>("canvas-widget-container");
        _uiTookitWidgetScene = root.Q<VisualElement>("ui-toolkit-widget-container");

        _anchorLabel = root.Q<Label>("anchor-label");
        _ualLabel = root.Q<Label>("ual-label");
        _atomicAssetLabel = root.Q<Label>("atomic-asset-label");
        _atomicMarketLabel = root.Q<Label>("atomic-market-label");
        _hyperionLabel = root.Q<Label>("hyperion-label");
        _quitLabel = root.Q<Label>("quit-label");

        _closeViewButton = root.Q<Button>("close-view-button");

        //_menu.RegisterCallback<TransitionEndEvent>(MenuTransitionEnd);

        StartCoroutine(PopupMenuAnimation());
        BindButtons();
    }

    #region Button Binding

    private void BindButtons()
    {
        _closeViewButton.clickable.clicked += () =>
        {
            if (_currentSceneLoaded != "")
            {
                SceneManager.UnloadSceneAsync(_currentSceneLoaded);
                _currentSceneLoaded = "";

                _maskBox.Hide();

                _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 254, 0)));
            }
        };

        _anchorLabel.RegisterCallback<ClickEvent>(evt =>
        {
            _clickedButtonName = _anchorLabel.text;

            _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 0, 0)));
        });

        _ualLabel.RegisterCallback<ClickEvent>(evt =>
        {
            _clickedButtonName = _ualLabel.text;

            _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 0, 0)));
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

    //private void MenuTransitionEnd(TransitionEndEvent evt)
    //{
    //    if (!evt.stylePropertyNames.Contains("opacity")) { return; }

    //    if (_mainPopupIndex < _mainMenuOptions.Length - 1)
    //    {
    //        _mainPopupIndex++;

    //        _mainMenuOptions[_mainPopupIndex].ToggleInClassList(PopUpAnimation);
    //    }
    //}

    private IEnumerator PopupMenuAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        _menu.ToggleInClassList(PopUpAnimation);
    }

    private void LoadScene(string targetScene)
    {
        _maskBox.Show();
        _currentSceneLoaded = targetScene;
        SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    #endregion
}


