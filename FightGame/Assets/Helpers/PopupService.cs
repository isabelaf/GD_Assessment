using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Helpers
{
    public class PopupService
    {
        private readonly Dictionary<int, Popup> popups = new Dictionary<int, Popup>();

        public void ShowPopup(int popupId, string text, List<(string, Action)> buttons)
        {
            if (!popups.ContainsKey(popupId))
                popups.Add(popupId, new Popup(popupId, text, buttons));
            //if (popups.ContainsKey(popupId) && popups[popupId].Text != text)
            //    popups[popupId] = new Popup(popupId, text, buttons);

            popups[popupId].Show();
        }
    }

    public class PopupStyle
    {
        public static GUIStyle LabelStyle = InitLabelStyle();
        public static GUIStyle ButtonStyle = InitButtonStyle();

        public static readonly float PaddingTop = 20f;
        public static readonly float PaddingLeftRight = 20f;
        public static readonly float ButtonHeight = 50f;

        private static GUIStyle InitLabelStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                wordWrap = true
            };
        }

        private static GUIStyle InitButtonStyle()
        {
            return new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                wordWrap = true
            };
        }
    }

    public class Popup
    {
        private readonly int id;
        private readonly string text;
        private readonly List<(string, Action)> buttons;

        private Rect popupRect, contentRect, labelRect, scrollViewPositionRect;
        private Rect[] buttonRects;
        private Vector2 scrollPosition;

        public string Text => text;

        public Popup(int id, string text, List<(string, Action)> buttons)
        {
            this.id = id;
            this.text = text;
            this.buttons = buttons;

            InitMeasurements();
        }

        private void InitMeasurements()
        {
            (popupRect, contentRect, labelRect) = GetContentRects(text);

            scrollViewPositionRect = new Rect(0, 0, popupRect.width, popupRect.height);

            buttonRects = new Rect[buttons.Count];
            for (var i = 0; i < buttons.Count; i++)
            {
                buttonRects[i] = GetButtonRect(contentRect.size, i, buttons.Count);
            }

            scrollPosition = Vector2.zero;
        }

        public void Show()
        {
            GUI.Window(id, popupRect, _ =>
            {
                scrollPosition = GUI.BeginScrollView(scrollViewPositionRect, scrollPosition, contentRect);

                GUI.Label(labelRect, text, PopupStyle.LabelStyle);

                for (var i = 0; i < buttons.Count; i++)
                {
                    var button = buttons[i];

                    if (GUI.Button(buttonRects[i], button.Item1, PopupStyle.ButtonStyle))
                    {
                        button.Item2();
                    }
                }

                GUI.EndScrollView();
            }, "");
        }

        private static Rect GetPopupRect(float height = 0f)
        {
            float y = Screen.height / 4;

            if (height == 0f)
                height = Screen.height * 0.5f;
            if (height != 0f)
                y = Screen.height / 2 - height / 2;

            var size = new Vector2
            {
                x = Screen.width * 0.5f,
                y = height
            };

            var position = new Vector2
            {
                x = Screen.width / 4,
                y = y
            };

            return new Rect(position, size);
        }

        private static Rect GetButtonRect(Vector2 contentSize, int buttonIndex, int buttonsCount)
        {
            var size = new Vector2
            {
                x = contentSize.x / buttonsCount,
                y = PopupStyle.ButtonHeight
            };

            var position = new Vector2
            {
                x = size.x * buttonIndex,
                y = contentSize.y - PopupStyle.ButtonHeight
            };

            return new Rect(position, size);
        }

        private static (Rect, Rect, Rect) GetContentRects(string text)
        {
            var popupRect = GetPopupRect();

            var width = popupRect.width - PopupStyle.PaddingLeftRight * 2 - GUI.skin.verticalScrollbar.fixedWidth;
            var labelRect = new Rect(
                PopupStyle.PaddingLeftRight,
                PopupStyle.PaddingTop,
                width,
                PopupStyle.LabelStyle.CalcHeight(new GUIContent { text = text }, width));

            var contentRect = new Rect(
                0,
                0,
                popupRect.width,
                2 * PopupStyle.PaddingTop + labelRect.height + PopupStyle.ButtonHeight);

            if (popupRect.height < contentRect.height)
            {
                contentRect.width -= GUI.skin.verticalScrollbar.fixedWidth + 1;
            }
            else
            {
                labelRect.width += GUI.skin.verticalScrollbar.fixedWidth;
                popupRect = GetPopupRect(contentRect.height);

            }

            return (popupRect, contentRect, labelRect);
        }
    }
}
