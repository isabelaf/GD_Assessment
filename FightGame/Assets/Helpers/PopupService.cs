using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Helpers
{
    public class PopupService
    {
        public static void ShowConfirmationPopup(int popupId, string text, List<(string, Action)> buttons, bool isSmall = false)
        {
            var popupRect = GetPopupRect(isSmall);

            GUI.Window(popupId, popupRect, _ =>
            {
                for (var i = 0; i < buttons.Count; i++)
                {
                    var button = buttons[i];

                    if (GUI.Button(GetButtonRect(popupRect, i, buttons.Count), button.Item1))
                    {
                        button.Item2();
                    }
                }
            }, text, GetPopupStyle());
        }

        private static Rect GetPopupRect(bool isSmall = false)
        {
            var sizeFactor = !isSmall ? 0.75f : 0.5f;
            var size = new Vector2
            {
                x = Screen.width * sizeFactor,
                y = Screen.height * sizeFactor
            };

            float positionFactor = !isSmall ? 8 : 4;
            var position = new Vector2
            {
                x = Screen.width / positionFactor,
                y = Screen.height / positionFactor
            };

            return new Rect(position, size);
        }

        private static Rect GetButtonRect(Rect popupRect, int buttonIndex, int buttonCount)
        {
            var size = new Vector2
            {
                x = popupRect.size.x / buttonCount,
                y = 50
            };

            var position = new Vector2
            {
                x = size.x * buttonIndex,
                y = popupRect.size.y - size.y - 10
            };

            return new Rect(position, size);
        }

        private static GUIStyle GetPopupStyle()
        {
            var popupStyle = new GUIStyle(GUI.skin.window)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                wordWrap = true
            };

            return popupStyle;
        }
    }
}
