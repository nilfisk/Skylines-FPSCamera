﻿using System;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace FPSCamera
{

    public class Mod : IUserMod
    {

        public string Name
        {
            get { return "First Person Camera"; }
        }

        public string Description
        {
            get { return "See your city from a different perspective"; }
        }

    }

    public class ModTerrainUtil : TerrainExtensionBase
    {

        private static ITerrain terrain = null;

        public static float GetHeight(float x, float z)
        {
            if (terrain == null)
            {
                return 0.0f;
            }

            return terrain.SampleTerrainHeight(x, z);
        }

        public override void OnCreated(ITerrain _terrain)
        {
            terrain = _terrain;
        }
    }

    public class ModLoad : LoadingExtensionBase
    {
        private UIButton cameraModeButton;
        private UILabel cameraModeLabel;

        public override void OnLevelLoaded(LoadMode mode)
        {
            var uiView = GameObject.FindObjectOfType<UIView>();

            cameraModeButton = uiView.AddUIComponent(typeof(UIButton)) as UIButton;

            cameraModeButton.width = 36;
            cameraModeButton.height = 36;

            cameraModeButton.pressedBgSprite = "OptionBasePressed";
            cameraModeButton.normalBgSprite = "OptionBase";
            cameraModeButton.hoveredBgSprite = "OptionBaseHovered";
            cameraModeButton.disabledBgSprite = "OptionBaseDisabled";

            cameraModeButton.normalFgSprite = "InfoPanelIconFreecamera";
            cameraModeButton.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            cameraModeButton.scaleFactor = 1.0f;

            cameraModeButton.tooltip = "FPS Camera configuration";
            cameraModeButton.tooltipBox = uiView.defaultTooltipBox;

            UIComponent escbutton = uiView.FindUIComponent("Esc");
            cameraModeButton.relativePosition = new Vector2
            (
                escbutton.relativePosition.x + escbutton.width / 2.0f - cameraModeButton.width / 2.0f - escbutton.width - 8.0f,
                escbutton.relativePosition.y + escbutton.height / 2.0f - cameraModeButton.height / 2.0f
            );

            cameraModeButton.eventClick += ButtonClick;

            var labelObject = new GameObject();
            labelObject.transform.parent = uiView.transform;

            cameraModeLabel = labelObject.AddComponent<UILabel>();
            cameraModeLabel.textColor = new Color32(255, 255, 255, 255);
            cameraModeLabel.transformPosition = new Vector3(1.15f, 0.90f);
            cameraModeLabel.Hide();

            FPSCamera.Initialize();
            FPSCamera.onCameraModeChanged = state =>
            {
                if (state)
                {
                    cameraModeLabel.text = String.Format("Press ({0}) to exit first-person mode", FPSCamera.GetToggleUIKey());
                    cameraModeLabel.Show();
                }
                else
                {
                    cameraModeLabel.Hide();
                }
            };
        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            FPSCamera.ToggleUI();
        }

    }

}
