using System;
using YanZhiwei.DotNet2.Utilities.Result;
using YanZhiwei.DotNet3._5.Utilities.Model;
using YanZhiwei.DotNet3._5.Utilities.WebForm.Core;

namespace YanZhiwei.DotNet3._5.Utilities.WebFormExamples
{
    public partial class WebUploadFileDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files.Count > 0)
            {
                WebUploadFile _uploadFile = new WebUploadFile();
                _uploadFile.SetFileDirectory("/上传");
                OperatedResult<UploadFileInfo> _uploadFileResult = _uploadFile.Save(Request.Files["File1"]);
                Label1.Text = _uploadFileResult.State == true ? string.Format("上传成功，保存路径：『{0}』", _uploadFileResult.Data.FilePath) : "上传失败" + _uploadFileResult.Message;
            }
        }
    }
}