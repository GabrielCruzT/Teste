using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StreamView : MonoBehaviour
{
    public ObButton depthButton;
    public ObButton colorButton;
    public ObButton bodyButton;
    public ObButton maskedColorButton;
    public ObButton colorizedBodyButton;

    void Awake()
    {
        StreamViewModel viewModel = StreamViewModel.Instance;

        depthButton.onClick.AddListener(() =>
        {
            viewModel.depthStream.Value = !viewModel.depthStream.Value;
            depthButton.OnOff(viewModel.depthStream.Value);
        });
        colorButton.onClick.AddListener(() =>
        {
            viewModel.colorStream.Value = !viewModel.colorStream.Value;
            colorButton.OnOff(viewModel.colorStream.Value);
        });
        bodyButton.onClick.AddListener(() =>
        {
            viewModel.bodyStream.Value = !viewModel.bodyStream.Value;
            bodyButton.OnOff(viewModel.bodyStream.Value);
        });
        maskedColorButton.onClick.AddListener(() =>
        {
            viewModel.maskedColorStream.Value = !viewModel.maskedColorStream.Value;
            maskedColorButton.OnOff(viewModel.maskedColorStream.Value);
        });
        colorizedBodyButton.onClick.AddListener(() =>
        {
            viewModel.colorizedBodyStream.Value = !viewModel.colorizedBodyStream.Value;
            colorizedBodyButton.OnOff(viewModel.colorizedBodyStream.Value);
        });

        AstraManager.Instance.OnInitializeFailed.AddListener(() =>
        {
            depthButton.interactable = false;
            colorButton.interactable = false;
            bodyButton.interactable = false;
            maskedColorButton.interactable = false;
            colorizedBodyButton.interactable = false;
        });

        AstraManager.Instance.OnInitializeSuccess.AddListener(() =>
        {
            viewModel.depthStream.Value = true;
            depthButton.OnOff(viewModel.depthStream.Value);

            var pid = AstraManager.Instance.DepthStream.usbInfo.Pid;
            if (pid == Constant.BUS_CL_PID)
            {
                colorButton.interactable = false;
                maskedColorButton.interactable = false;
                colorizedBodyButton.interactable = false;
            }
            else
            {
                viewModel.colorStream.Value = true;
                colorButton.OnOff(viewModel.colorStream.Value);
            }
        });
    }
}
