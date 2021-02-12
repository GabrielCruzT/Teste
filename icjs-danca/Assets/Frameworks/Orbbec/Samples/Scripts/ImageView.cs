using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ImageView : MonoBehaviour
{
	public RawImage depthImage;
	public RawImage colorImage;
	public RawImage maskedColorImage;
	public RawImage colorizedBodyImage;

    // Use this for initialization
    void Awake()
    {
		StreamViewModel viewModel = StreamViewModel.Instance;
		viewModel.depthStream.onValueChanged += OnDepthStreamChanged;
		viewModel.colorStream.onValueChanged += OnColorStreamChanged;
		viewModel.colorizedBodyStream.onValueChanged += OnColorizedBodyStreamChanged;
		viewModel.maskedColorStream.onValueChanged += OnMaskedColorStreamChanged;

		depthImage.gameObject.SetActive(false);
		colorImage.gameObject.SetActive(false);
		colorizedBodyImage.gameObject.SetActive(false);
		maskedColorImage.gameObject.SetActive(false);
    }

    private void OnDepthStreamChanged(bool value)
    {
        if(value)
		{
			depthImage.gameObject.SetActive(true);
			depthImage.texture = AstraManager.Instance.DepthTexture;
		}
		else
		{
			depthImage.gameObject.SetActive(false);
		}
    }

    private void OnColorStreamChanged(bool value)
    {
        if(value)
		{
			colorImage.gameObject.SetActive(true);
			colorImage.texture = AstraManager.Instance.ColorTexture;
		}
		else
		{
			colorImage.gameObject.SetActive(false);
		}
    }

    private void OnColorizedBodyStreamChanged(bool value)
    {
        if(value)
		{
			colorizedBodyImage.gameObject.SetActive(true);
			colorizedBodyImage.texture = AstraManager.Instance.ColorizedBodyTexture;
		}
		else
		{
			colorizedBodyImage.gameObject.SetActive(false);
		}
    }

    private void OnMaskedColorStreamChanged(bool value)
    {
        if(value)
		{
			maskedColorImage.gameObject.SetActive(true);
			maskedColorImage.texture = AstraManager.Instance.MaskedColorTexture;
		}
		else
		{
			maskedColorImage.gameObject.SetActive(false);
		}
    }
}
