﻿using System;
using System.Collections.Generic;
using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Utils;
using UnityEngine;
using Hover.Common.Items.Types;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererSlider : MonoBehaviour {
	
		public GameObject Container;
		public HoverRendererHollowRectangle[] Backgrounds;
		public HoverRendererButton HandleButton;
		public HoverRendererButton JumpButton;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float ZeroValue = 0.5f;
				
		[Range(0, 1)]
		public float HandleValue = 0.5f;
		
		[Range(0, 1)]
		public float JumpValue = 0;
		
		public bool ShowJump = false;
		public SliderItem.FillType FillType = SliderItem.FillType.Zero;
		
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		
		private readonly List<SliderUtil.Segment> vSegments;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererSlider() {
			vSegments = new List<SliderUtil.Segment>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !vIsBuilt ) {
				BuildElements();
				vIsBuilt = true;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateSliderSegments();
			UpdateGeneralSettings();
			UpdateAnchorSettings();

			foreach ( HoverRendererHollowRectangle background in Backgrounds ) {
				if ( background.gameObject.activeSelf ) {
					background.UpdateAfterRenderer();
				}
			}
			
			HandleButton.UpdateAfterRenderer();
			JumpButton.UpdateAfterRenderer();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Container = new GameObject("Container");
			Container.transform.SetParent(gameObject.transform, false);
			
			Backgrounds = new HoverRendererHollowRectangle[4];
			
			for ( int i = 0 ; i < Backgrounds.Length ; i++ ) {
				HoverRendererHollowRectangle background = BuildHollowRect("Background"+i);
				background.FillColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
				Backgrounds[i] = background;
			}
			
			HandleButton = BuildButton("Handle");
			JumpButton = BuildButton("Jump");
			
			HandleButton.SizeY = 2;
			JumpButton.SizeY = 1;
			
			JumpButton.Canvas.gameObject.SetActive(false);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererHollowRectangle BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverRendererHollowRectangle>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererButton BuildButton(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(Container.transform, false);
			return rectGo.AddComponent<HoverRendererButton>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSliderSegments() {
			var info = new SliderUtil.SliderInfo {
				FillType = FillType,
				TrackStartPosition = -SizeY/2,
				TrackEndPosition = SizeY/2,
				HandleSize = HandleButton.SizeY,
				HandleValue = HandleValue,
				JumpSize = (ShowJump ? JumpButton.SizeY : 0),
				JumpValue = JumpValue,
				ZeroValue = ZeroValue,
			};
			
			SliderUtil.CalculateSegments(info, vSegments);
			
			/*Debug.Log("INFO: "+info.TrackStartPosition+" / "+info.TrackEndPosition);
			
			foreach ( SliderUtil.Segment seg in vSegments ) {
				Debug.Log(" - "+seg.Type+": "+seg.StartPosition+" / "+seg.EndPosition);
			}*/
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			int bgIndex = 0;
			bool isJumpSegmentVisible = false;
			
			foreach ( HoverRendererHollowRectangle background in Backgrounds ) {
				background.SizeY = 0;
			}
			
			foreach ( SliderUtil.Segment seg in vSegments ) {
				switch ( seg.Type ) {
					case SliderUtil.SegmentType.Track:
						HoverRendererHollowRectangle background = Backgrounds[bgIndex++];
						background.SizeY = seg.EndPosition-seg.StartPosition;
						background.transform.localPosition = 
							new Vector3(0, (seg.StartPosition+seg.EndPosition)/2, 0);
						break;
				
					case SliderUtil.SegmentType.Handle:
					case SliderUtil.SegmentType.Jump:
						HoverRendererButton button = 
							(seg.Type == SliderUtil.SegmentType.Handle ? HandleButton : JumpButton);
						button.SizeY = seg.EndPosition-seg.StartPosition;
						button.transform.localPosition = 
							new Vector3(0, (seg.StartPosition+seg.EndPosition)/2, 0);
						break;
				}
				
				if ( seg.Type == SliderUtil.SegmentType.Jump ) {
					isJumpSegmentVisible = true;
				}
			}
			
			HandleButton.ControlledByRenderer = true;
			JumpButton.ControlledByRenderer = true;
			
			foreach ( HoverRendererHollowRectangle background in Backgrounds ) {
				background.ControlledByRenderer = true;
				background.SizeX = SizeX*0.8f;
				background.InnerAmount = 0;
				background.gameObject.SetActive(background.SizeY > 0);
			}
			
			JumpButton.gameObject.SetActive(ShowJump && isJumpSegmentVisible);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			int ai = (int)Anchor;
			float x = (ai%3)/2f - 0.5f;
			float y = (ai/3)/2f - 0.5f;
			var localPos = new Vector3(-SizeX*x, SizeY*y, 0);
			
			Container.transform.localPosition = localPos;
		}
		
	}

}
