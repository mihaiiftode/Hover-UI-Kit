﻿using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public interface IUiMenuPointRenderer {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Build(ArcState pArcState, ArcSegmentState pSegState, float pAlpha0, float pAlpha1);
		
		/*--------------------------------------------------------------------------------------------*/
		void Update();

		/*--------------------------------------------------------------------------------------------*/
		void HandleChangeAnimation(bool pFadeIn, int pDirection, float pProgress);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float CalculateCursorDistance(Vector3 pCursorPosition);

	}

}
