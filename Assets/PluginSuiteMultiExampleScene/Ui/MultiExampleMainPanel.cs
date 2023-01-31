using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnchorLinkTransportSharp.Examples.Canvas;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using AnchorLinkTransportSharp.Examples.UiToolkit.Ui;
using UniversalAuthenticatorLibrary.Examples.Canvas;
using UniversalAuthenticatorLibrary.Examples.UiToolkit.Ui;
using WaxCloudWalletUnity.Examples.Ui;

public class MultiExampleMainPanel : MonoBehaviour
{
    /**
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
    private Label _wcwLabel;
    private Label _atomicAssetLabel;
    private Label _atomicMarketLabel;
    private Label _hyperionLabel;
    private Label _quitLabel;

    private Button _closeViewButton;

    /**
     * Fields/Properties
     */
    private const string PopUpAnimation = "pop-animation-hide";

    private string _currentSceneLoaded = "";
    private string _clickedButtonName = string.Empty;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _menu = root.Q<VisualElement>("menu");
        _maskBox = root.Q<VisualElement>("mask-box");
        _maskBox.style.visibility = Visibility.Hidden;
        _maskBox.style.display = DisplayStyle.None;

        _mainMenuOptions = _menu.Q<VisualElement>("main-nav").Children().ToArray();
        _widgets = root.Q<VisualElement>("body").Children().ToList();
        _canvasWidgetScene = root.Q<VisualElement>("canvas-widget-container");
        _uiTookitWidgetScene = root.Q<VisualElement>("ui-toolkit-widget-container");

        _anchorLabel = root.Q<Label>("anchor-label");
        _ualLabel = root.Q<Label>("ual-label");
        _wcwLabel = root.Q<Label>("wcw-label");
        _atomicAssetLabel = root.Q<Label>("atomic-asset-label");
        _atomicMarketLabel = root.Q<Label>("atomic-market-label");
        _hyperionLabel = root.Q<Label>("hyperion-label");
        _quitLabel = root.Q<Label>("quit-label");

        _closeViewButton = root.Q<Button>("close-view-button");

#if UNITY_2021_0_OR_NEWER
        _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 254, 0)));
#else
        _widgets.ForEach(x => x.style.visibility = Visibility.Hidden);
#endif

        //_wcwLabel.style.visibility = Visibility.Hidden;
        //_wcwLabel.style.display = DisplayStyle.None;

#if UNITY_WEBGL
        _quitLabel.style.visibility = Visibility.Hidden;
        _quitLabel.style.display = DisplayStyle.None;
        
        _wcwLabel.style.visibility = Visibility.Visible;
        _wcwLabel.style.display = DisplayStyle.Flex;
#endif
        StartCoroutine(PopupMenuAnimation());
        BindButtons();
    }

    #region Button Binding
    /// <summary>
    /// Binds all the event interaction 
    /// </summary>
    private void BindButtons()
    {
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

        _wcwLabel.RegisterCallback<ClickEvent>(evt =>
        {
            LoadScene("WaxCloudWalletExampleScene");
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
                    LoadScene("CanvasAnchorExampleScene");
                    break;
                case "UAL":
                    LoadScene("CanvasUALExampleScene");
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
    /// <summary>
    /// Populate the menu in animated style
    /// </summary>
    /// <returns></returns>
    private IEnumerator PopupMenuAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        _menu.ToggleInClassList(PopUpAnimation);
    }

    /// <summary>
    /// Load scene that is being pass by the scene name as a string
    /// </summary>
    /// <param name="targetScene"></param>
    private void LoadScene(string targetScene)
    {
        _maskBox.style.visibility = Visibility.Visible;
        _maskBox.style.display = DisplayStyle.Flex;

        _currentSceneLoaded = targetScene;
        SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called when ctrl + v is pressed in browser (webgl)
    /// </summary>
    /// <param name="pastedText">The pasted text.</param>
    public void OnClipboardPaste(string pastedText)
    {
        if (string.IsNullOrEmpty(pastedText))
            return;

        switch (_currentSceneLoaded)
        {
            /* UI Toolkit targets */
            case "UiToolkitAnchorExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<AnchorExamplePanel>())
                            .GetComponentInChildren<AnchorExamplePanel>().OnBrowserClipboardPaste(pastedText);
                break;
            case "UiToolkitUALExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<UALExamplePanel>())
                            .GetComponentInChildren<UALExamplePanel>().OnBrowserClipboardPaste(pastedText);
                break;
            case "WaxCloudWalletExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<WaxCloudWalletMainPanel>())
                            .GetComponentInChildren<WaxCloudWalletMainPanel>().OnBrowserClipboardPaste(pastedText);
                break;
            case "AtomicAssetsExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<AtomicAssetsExamplePanel>())
                            .GetComponentInChildren<AtomicAssetsExamplePanel>().OnBrowserClipboardPaste(pastedText);
                break;
            case "AtomicMarketExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<AtomicMarketExamplePanel>())
                            .GetComponentInChildren<AtomicMarketExamplePanel>().OnBrowserClipboardPaste(pastedText);
                break;
            case "HyperionExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<HyperionExamplePanel>())
                            .GetComponentInChildren<HyperionExamplePanel>().OnBrowserClipboardPaste(pastedText);
                break;
            /* Canvas targets */
            case "CanvasAnchorExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<CanvasExample>())
                            .GetComponentInChildren<CanvasExample>().OnBrowserClipboardPaste(pastedText);
                print("This function to paste is being called!");
                break;
            case "CanvasUALExampleScene":
                SceneManager.GetSceneByName(_currentSceneLoaded).GetRootGameObjects().ToList()
                            .Find(child => child.GetComponentInChildren<UALCanvasExample>())
                            .GetComponentInChildren<UALCanvasExample>().OnBrowserClipboardPaste(pastedText);
                print("This function to paste is being called!");
                break;
        }


    }
    #endregion
}


