/*============================================================================
                    NVIDIA FXAA 3.11 by TIMOTHY LOTTES
------------------------------------------------------------------------------
COPYRIGHT (C) 2010, 2011 NVIDIA CORPORATION. ALL RIGHTS RESERVED.
------------------------------------------------------------------------------
TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THIS SOFTWARE IS PROVIDED
*AS IS* AND NVIDIA AND ITS SUPPLIERS DISCLAIM ALL WARRANTIES, EITHER EXPRESS
OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL NVIDIA
OR ITS SUPPLIERS BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT, OR
CONSEQUENTIAL DAMAGES WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR
LOSS OF BUSINESS PROFITS, BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION,
OR ANY OTHER PECUNIARY LOSS) ARISING OUT OF THE USE OF OR INABILITY TO USE
THIS SOFTWARE, EVEN IF NVIDIA HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH
DAMAGES.
============================================================================*/

#if (FXAA_QUALITY__PRESET == 12)
    #define FXAA_QUALITY__PS 5
    #define FXAA_QUALITY__P0 1.0
    #define FXAA_QUALITY__P1 1.5
    #define FXAA_QUALITY__P2 2.0
    #define FXAA_QUALITY__P3 4.0
    #define FXAA_QUALITY__P4 12.0
#endif

#if (FXAA_QUALITY__PRESET == 23)
    #define FXAA_QUALITY__PS 6
    #define FXAA_QUALITY__P0 1.0
    #define FXAA_QUALITY__P1 1.5
    #define FXAA_QUALITY__P2 2.0
    #define FXAA_QUALITY__P3 2.0
    #define FXAA_QUALITY__P4 2.0
    #define FXAA_QUALITY__P5 8.0
#endif

#if (FXAA_QUALITY__PRESET == 39)
    #define FXAA_QUALITY__PS 12
    #define FXAA_QUALITY__P0 1.0
    #define FXAA_QUALITY__P1 1.0
    #define FXAA_QUALITY__P2 1.0
    #define FXAA_QUALITY__P3 1.0
    #define FXAA_QUALITY__P4 1.0
    #define FXAA_QUALITY__P5 1.5
    #define FXAA_QUALITY__P6 2.0
    #define FXAA_QUALITY__P7 2.0
    #define FXAA_QUALITY__P8 2.0
    #define FXAA_QUALITY__P9 2.0
    #define FXAA_QUALITY__P10 4.0
    #define FXAA_QUALITY__P11 8.0
#endif


in vec3 v2f_WorldPos;
in vec2 v2f_Texcoord;

out vec4 FragColor;

uniform sampler2D ScreenTexture;
uniform vec2 RcpFrame;
uniform float Subpix;
uniform float EdgeThreshold;
uniform float EdgeThresholdMin;

vec4 FXAA_PC()
{
    #define FxaaSat(x) clamp(x, 0.0, 1.0)
    #define FxaaLuma(x) x.a 
    #define FxaaTexTop(t, p) textureLod(t, p, 0.0)
    #define FxaaTexOff(t, p, o, r) textureLodOffset(t, p, 0.0, o)

    vec2 posM = v2f_Texcoord;
    vec4 rgbyM = FxaaTexTop(ScreenTexture, posM);

    #define lumaM rgbyM.w

    float lumaS = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2( 0, 1), RcpFrame.xy));
    float lumaE = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2( 1, 0), RcpFrame.xy));
    float lumaN = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2( 0,-1), RcpFrame.xy));
    float lumaW = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2(-1, 0), RcpFrame.xy));

    /*--------------------------------------------------------------------------*/
        float maxSM = max(lumaS, lumaM);
        float minSM = min(lumaS, lumaM);
        float maxESM = max(lumaE, maxSM);
        float minESM = min(lumaE, minSM);
        float maxWN = max(lumaN, lumaW);
        float minWN = min(lumaN, lumaW);
        float rangeMax = max(maxWN, maxESM);
        float rangeMin = min(minWN, minESM);
        float rangeMaxScaled = rangeMax * EdgeThreshold;
        float range = rangeMax - rangeMin;
        float rangeMaxClamped = max(EdgeThresholdMin, rangeMaxScaled);
        bool earlyExit = range < rangeMaxClamped;
    /*--------------------------------------------------------------------------*/
        if(earlyExit)
            return rgbyM;
    /*--------------------------------------------------------------------------*/
        #if (FXAA_GATHER4_ALPHA == 0)
            float lumaNW = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2(-1,-1), RcpFrame.xy));
            float lumaSE = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2( 1, 1), RcpFrame.xy));
            float lumaNE = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2( 1,-1), RcpFrame.xy));
            float lumaSW = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2(-1, 1), RcpFrame.xy));
        #else
            float lumaNE = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2(1, -1), RcpFrame.xy));
            float lumaSW = FxaaLuma(FxaaTexOff(ScreenTexture, posM, ivec2(-1, 1), RcpFrame.xy));
        #endif
    /*--------------------------------------------------------------------------*/
        float lumaNS = lumaN + lumaS;
        float lumaWE = lumaW + lumaE;
        float subpixRcpRange = 1.0/range;
        float subpixNSWE = lumaNS + lumaWE;
        float edgeHorz1 = (-2.0 * lumaM) + lumaNS;
        float edgeVert1 = (-2.0 * lumaM) + lumaWE;
    /*--------------------------------------------------------------------------*/
        float lumaNESE = lumaNE + lumaSE;
        float lumaNWNE = lumaNW + lumaNE;
        float edgeHorz2 = (-2.0 * lumaE) + lumaNESE;
        float edgeVert2 = (-2.0 * lumaN) + lumaNWNE;
    /*--------------------------------------------------------------------------*/
        float lumaNWSW = lumaNW + lumaSW;
        float lumaSWSE = lumaSW + lumaSE;
        float edgeHorz4 = (abs(edgeHorz1) * 2.0) + abs(edgeHorz2);
        float edgeVert4 = (abs(edgeVert1) * 2.0) + abs(edgeVert2);
        float edgeHorz3 = (-2.0 * lumaW) + lumaNWSW;
        float edgeVert3 = (-2.0 * lumaS) + lumaSWSE;
        float edgeHorz = abs(edgeHorz3) + edgeHorz4;
        float edgeVert = abs(edgeVert3) + edgeVert4;
    /*--------------------------------------------------------------------------*/
        float subpixNWSWNESE = lumaNWSW + lumaNESE;
        float lengthSign = RcpFrame.x;
        bool horzSpan = edgeHorz >= edgeVert;
        float subpixA = subpixNSWE * 2.0 + subpixNWSWNESE;
    /*--------------------------------------------------------------------------*/
        if(!horzSpan) lumaN = lumaW;
        if(!horzSpan) lumaS = lumaE;
        if(horzSpan) lengthSign = RcpFrame.y;
        float subpixB = (subpixA * (1.0/12.0)) - lumaM;
    /*--------------------------------------------------------------------------*/
        float gradientN = lumaN - lumaM;
        float gradientS = lumaS - lumaM;
        float lumaNN = lumaN + lumaM;
        float lumaSS = lumaS + lumaM;
        bool pairN = abs(gradientN) >= abs(gradientS);
        float gradient = max(abs(gradientN), abs(gradientS));
        if(pairN) lengthSign = -lengthSign;
        float subpixC = FxaaSat(abs(subpixB) * subpixRcpRange);
    /*--------------------------------------------------------------------------*/
        vec2 posB;
        posB.x = posM.x;
        posB.y = posM.y;
        vec2 offNP;
        offNP.x = (!horzSpan) ? 0.0 : RcpFrame.x;
        offNP.y = ( horzSpan) ? 0.0 : RcpFrame.y;
        if(!horzSpan) posB.x += lengthSign * 0.5;
        if( horzSpan) posB.y += lengthSign * 0.5;
    /*--------------------------------------------------------------------------*/
        vec2 posN;
        posN.x = posB.x - offNP.x * FXAA_QUALITY__P0;
        posN.y = posB.y - offNP.y * FXAA_QUALITY__P0;
        vec2 posP;
        posP.x = posB.x + offNP.x * FXAA_QUALITY__P0;
        posP.y = posB.y + offNP.y * FXAA_QUALITY__P0;
        float subpixD = ((-2.0)*subpixC) + 3.0;
        float lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN));
        float subpixE = subpixC * subpixC;
        float lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP));
    /*--------------------------------------------------------------------------*/
        if(!pairN) lumaNN = lumaSS;
        float gradientScaled = gradient * 1.0/4.0;
        float lumaMM = lumaM - lumaNN * 0.5;
        float subpixF = subpixD * subpixE;
        bool lumaMLTZero = lumaMM < 0.0;
    /*--------------------------------------------------------------------------*/
        lumaEndN -= lumaNN * 0.5;
        lumaEndP -= lumaNN * 0.5;
        bool doneN = abs(lumaEndN) >= gradientScaled;
        bool doneP = abs(lumaEndP) >= gradientScaled;
        if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P1;
        if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P1;
        bool doneNP = (!doneN) || (!doneP);
        if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P1;
        if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P1;
    /*--------------------------------------------------------------------------*/
        if(doneNP) {
            if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
            if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
            if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
            if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
            doneN = abs(lumaEndN) >= gradientScaled;
            doneP = abs(lumaEndP) >= gradientScaled;
            if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P2;
            if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P2;
            doneNP = (!doneN) || (!doneP);
            if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P2;
            if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P2;
    /*--------------------------------------------------------------------------*/
            #if (FXAA_QUALITY__PS > 3)
            if(doneNP) {
                if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                doneN = abs(lumaEndN) >= gradientScaled;
                doneP = abs(lumaEndP) >= gradientScaled;
                if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P3;
                if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P3;
                doneNP = (!doneN) || (!doneP);
                if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P3;
                if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P3;
    /*--------------------------------------------------------------------------*/
                #if (FXAA_QUALITY__PS > 4)
                if(doneNP) {
                    if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                    if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                    if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                    if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                    doneN = abs(lumaEndN) >= gradientScaled;
                    doneP = abs(lumaEndP) >= gradientScaled;
                    if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P4;
                    if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P4;
                    doneNP = (!doneN) || (!doneP);
                    if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P4;
                    if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P4;
    /*--------------------------------------------------------------------------*/
                    #if (FXAA_QUALITY__PS > 5)
                    if(doneNP) {
                        if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                        if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                        if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                        if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                        doneN = abs(lumaEndN) >= gradientScaled;
                        doneP = abs(lumaEndP) >= gradientScaled;
                        if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P5;
                        if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P5;
                        doneNP = (!doneN) || (!doneP);
                        if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P5;
                        if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P5;
    /*--------------------------------------------------------------------------*/
                        #if (FXAA_QUALITY__PS > 6)
                        if(doneNP) {
                            if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                            if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                            if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                            if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                            doneN = abs(lumaEndN) >= gradientScaled;
                            doneP = abs(lumaEndP) >= gradientScaled;
                            if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P6;
                            if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P6;
                            doneNP = (!doneN) || (!doneP);
                            if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P6;
                            if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P6;
    /*--------------------------------------------------------------------------*/
                            #if (FXAA_QUALITY__PS > 7)
                            if(doneNP) {
                                if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                                if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                                if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                                if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                                doneN = abs(lumaEndN) >= gradientScaled;
                                doneP = abs(lumaEndP) >= gradientScaled;
                                if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P7;
                                if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P7;
                                doneNP = (!doneN) || (!doneP);
                                if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P7;
                                if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P7;
    /*--------------------------------------------------------------------------*/
        #if (FXAA_QUALITY__PS > 8)
        if(doneNP) {
            if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
            if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
            if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
            if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
            doneN = abs(lumaEndN) >= gradientScaled;
            doneP = abs(lumaEndP) >= gradientScaled;
            if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P8;
            if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P8;
            doneNP = (!doneN) || (!doneP);
            if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P8;
            if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P8;
    /*--------------------------------------------------------------------------*/
            #if (FXAA_QUALITY__PS > 9)
            if(doneNP) {
                if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                doneN = abs(lumaEndN) >= gradientScaled;
                doneP = abs(lumaEndP) >= gradientScaled;
                if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P9;
                if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P9;
                doneNP = (!doneN) || (!doneP);
                if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P9;
                if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P9;
    /*--------------------------------------------------------------------------*/
                #if (FXAA_QUALITY__PS > 10)
                if(doneNP) {
                    if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                    if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                    if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                    if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                    doneN = abs(lumaEndN) >= gradientScaled;
                    doneP = abs(lumaEndP) >= gradientScaled;
                    if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P10;
                    if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P10;
                    doneNP = (!doneN) || (!doneP);
                    if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P10;
                    if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P10;
    /*--------------------------------------------------------------------------*/
                    #if (FXAA_QUALITY__PS > 11)
                    if(doneNP) {
                        if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                        if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                        if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                        if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                        doneN = abs(lumaEndN) >= gradientScaled;
                        doneP = abs(lumaEndP) >= gradientScaled;
                        if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P11;
                        if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P11;
                        doneNP = (!doneN) || (!doneP);
                        if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P11;
                        if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P11;
    /*--------------------------------------------------------------------------*/
                        #if (FXAA_QUALITY__PS > 12)
                        if(doneNP) {
                            if(!doneN) lumaEndN = FxaaLuma(FxaaTexTop(ScreenTexture, posN.xy));
                            if(!doneP) lumaEndP = FxaaLuma(FxaaTexTop(ScreenTexture, posP.xy));
                            if(!doneN) lumaEndN = lumaEndN - lumaNN * 0.5;
                            if(!doneP) lumaEndP = lumaEndP - lumaNN * 0.5;
                            doneN = abs(lumaEndN) >= gradientScaled;
                            doneP = abs(lumaEndP) >= gradientScaled;
                            if(!doneN) posN.x -= offNP.x * FXAA_QUALITY__P12;
                            if(!doneN) posN.y -= offNP.y * FXAA_QUALITY__P12;
                            doneNP = (!doneN) || (!doneP);
                            if(!doneP) posP.x += offNP.x * FXAA_QUALITY__P12;
                            if(!doneP) posP.y += offNP.y * FXAA_QUALITY__P12;
    /*--------------------------------------------------------------------------*/
                        }
                        #endif
    /*--------------------------------------------------------------------------*/
                    }
                    #endif
    /*--------------------------------------------------------------------------*/
                }
                #endif
    /*--------------------------------------------------------------------------*/
            }
            #endif
    /*--------------------------------------------------------------------------*/
        }
        #endif
    /*--------------------------------------------------------------------------*/
                            }
                            #endif
    /*--------------------------------------------------------------------------*/
                        }
                        #endif
    /*--------------------------------------------------------------------------*/
                    }
                    #endif
    /*--------------------------------------------------------------------------*/
                }
                #endif
    /*--------------------------------------------------------------------------*/
            }
            #endif
    /*--------------------------------------------------------------------------*/
        }
    /*--------------------------------------------------------------------------*/
        float dstN = posM.x - posN.x;
        float dstP = posP.x - posM.x;
        if(!horzSpan) dstN = posM.y - posN.y;
        if(!horzSpan) dstP = posP.y - posM.y;
    /*--------------------------------------------------------------------------*/
        bool goodSpanN = (lumaEndN < 0.0) != lumaMLTZero;
        float spanLength = (dstP + dstN);
        bool goodSpanP = (lumaEndP < 0.0) != lumaMLTZero;
        float spanLengthRcp = 1.0/spanLength;
    /*--------------------------------------------------------------------------*/
        bool directionN = dstN < dstP;
        float dst = min(dstN, dstP);
        bool goodSpan = directionN ? goodSpanN : goodSpanP;
        float subpixG = subpixF * subpixF;
        float pixelOffset = (dst * (-spanLengthRcp)) + 0.5;
        float subpixH = subpixG * Subpix;
    /*--------------------------------------------------------------------------*/
        float pixelOffsetGood = goodSpan ? pixelOffset : 0.0;
        float pixelOffsetSubpix = max(pixelOffsetGood, subpixH);
        if(!horzSpan) posM.x += pixelOffsetSubpix * lengthSign;
        if( horzSpan) posM.y += pixelOffsetSubpix * lengthSign;

        return vec4(FxaaTexTop(ScreenTexture, posM).xyz, lumaM);
}

void main() 
{
    FragColor = FXAA_PC();
}