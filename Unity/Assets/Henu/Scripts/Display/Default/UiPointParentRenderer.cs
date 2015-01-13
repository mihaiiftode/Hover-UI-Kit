﻿using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public class UiPointParentRenderer : UiPointRenderer {

		public static float ArrowSize = 16;
		public static Texture2D ArrowTexture = Resources.Load<Texture2D>("Arrow");

		private GameObject vArrow;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(ArcState pArcState, ArcSegmentState pSegState,
																		float pAngle0, float pAngle1) {
			base.Build(pArcState, pSegState, pAngle0, pAngle1);

			vArrow = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vArrow.transform.SetParent(gameObject.transform, false);
			vArrow.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vArrow.renderer.sharedMaterial.color = Color.clear;
			vArrow.renderer.sharedMaterial.mainTexture = ArrowTexture;

			////
			
			int mult = (vArcState.IsLeft ? 1 : -1);

			vArrow.transform.localPosition = new Vector3(0, 0, 1+(Width-Height/2f)*Scale*mult);
			vArrow.transform.localRotation = vCanvasGroupObj.transform.localRotation;
			vArrow.transform.localScale = new Vector3(ArrowSize*Scale*mult, ArrowSize*Scale, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			float alpha = vSegState.HighlightProgress*0.8f + 0.2f;
			vArrow.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vMainAlpha);
		}

	}

}
