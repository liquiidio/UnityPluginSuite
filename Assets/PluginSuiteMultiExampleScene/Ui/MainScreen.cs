using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;
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

    //private Label _anchorLabel;
    //private Label _anchorLabel;
    //private Label _anchorLabel;

    private Button _closeButton;

    /*
     * Fields/Properties
     */
    private const string POPUP_ANIMATION = "pop-animation-hide";
    private string _currentSceneLoaded = "";
    private int _mainPopupIndex = -1;

    private bool _isAnchorClicked;
    private bool _isUalClicked;


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

        _closeButton = root.Q<Button>("close-view-button");

        _menu.RegisterCallback<TransitionEndEvent>(MenuTransitionEnd);

        StartCoroutine(PopupAnimation());
        BindButtons();

    }

    #region Button Binding

    private void BindButtons()
    {
        _closeButton.clickable.clicked += () =>
        {
            if (_currentSceneLoaded != "")
            {
                SceneManager.UnloadSceneAsync(_currentSceneLoaded);
                _currentSceneLoaded = "";

                _maskBox.style.visibility = Visibility.Hidden;
            }
        };

        _anchorLabel.RegisterCallback<ClickEvent>(evt =>
        {
            _isAnchorClicked = true;
            _isUalClicked = false;
            _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 0, 0)));
        });

        _ualLabel.RegisterCallback<ClickEvent>(evt =>
        {
            _isUalClicked = true;
            _isAnchorClicked = false;
            _widgets.ForEach(x => x.style.translate = new StyleTranslate(new Translate(0, 0, 0)));
        });

        _canvasWidgetScene.RegisterCallback<ClickEvent>(evt =>
        {
            if (_isAnchorClicked)
            {
                LoadScene("ExampleCanvasScene");
            }
            else if (_isUalClicked)
            {
                LoadScene("CanvasScene");
            }
        });

        _uiTookitWidgetScene.RegisterCallback<ClickEvent>(evt =>
        {
            if (_isAnchorClicked)
            {
                LoadScene("ExampleUiToolkitScene");
            }
            else if (_isUalClicked)
            {
                LoadScene("UiToolkitUALScene");
            }
        });
    }

    #endregion

    #region Others

    private void MenuTransitionEnd(TransitionEndEvent evt)
    {
        if (!evt.stylePropertyNames.Contains("opacity")) { return; }

        if (_mainPopupIndex < _mainMenuOptions.Length - 1)
        {
            _mainPopupIndex++;

            _mainMenuOptions[_mainPopupIndex].ToggleInClassList(POPUP_ANIMATION);
        }
    }

    private IEnumerator PopupAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        _menu.ToggleInClassList(POPUP_ANIMATION);
    }

    private void LoadScene(string targetScene)
    {
        _maskBox.style.visibility = Visibility.Visible;
        _currentSceneLoaded = targetScene;
        SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    #endregion
}


