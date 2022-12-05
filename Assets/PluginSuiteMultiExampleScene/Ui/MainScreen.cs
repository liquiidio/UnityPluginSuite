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
    private VisualElement _canvasWidgetScene;
    private VisualElement _uiTookitWidgetScene;
    private VisualElement[] _mainMenuOptions;
    private List<VisualElement> _widgets;

    private Label _anchorLabel;
    private Label _ualLabel;

    //private Label _anchorLabel;
    //private Label _anchorLabel;
    //private Label _anchorLabel;

    /*
     * Fields/Properties
     */
    private const string POPUP_ANIMATION = "pop-animation-hide";
    private int _mainPopupIndex = -1;

    private bool _isAnchorClicked;
    private bool _isUalClicked;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _menu = root.Q<VisualElement>("menu");
        _canvasWidgetScene = root.Q<VisualElement>("canvas-widget-container");
        _uiTookitWidgetScene = root.Q<VisualElement>("ui-toolkit-widget-container");

        _anchorLabel = root.Q<Label>("anchor-label");
        _ualLabel = root.Q<Label>("ual-label");

        _mainMenuOptions = _menu.Q<VisualElement>("main-nav").Children().ToArray();
        _widgets = root.Q<VisualElement>("body").Children().ToList();

        _menu.RegisterCallback<TransitionEndEvent>(Menu_TransitionEnd);

        StartCoroutine(PopupAnimation());
        BindButtons();
    }

    #region Button Binding

    private void BindButtons()
    {
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
                SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
            }
            else if (_isUalClicked)
            {
                SceneManager.LoadScene("ExampleCanvasScene", LoadSceneMode.Additive);
            }
        });

        _uiTookitWidgetScene.RegisterCallback<ClickEvent>(evt =>
        {
            if (_isAnchorClicked)
            {
                SceneManager.LoadScene("ExampleUiToolkitScene", LoadSceneMode.Additive);
            }
            else if (_isUalClicked)
            {
                SceneManager.LoadScene("UiToolkitUALScene", LoadSceneMode.Additive);
            }
        });
    }

    #endregion

    #region Others

    private void Menu_TransitionEnd(TransitionEndEvent evt)
    {
        if (!evt.stylePropertyNames.Contains("opacity")) { return; }


        if (_mainPopupIndex < _mainMenuOptions.Length - 1)
        {
            _mainPopupIndex++;

            _mainMenuOptions[_mainPopupIndex].ToggleInClassList(POPUP_ANIMATION);
        }
        else
        {


        }
    }

    private IEnumerator PopupAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        _menu.ToggleInClassList(POPUP_ANIMATION);
    }

    #endregion
}
