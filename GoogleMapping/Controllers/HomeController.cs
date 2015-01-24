using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace GoogleMapping.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {                    
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string leftLong, string rightLong, string topLat, string bottomLat, string centerLong, string centerLat, string zoom)
        {
            if (file == null || file.ContentLength <= 0)
            {
                return Error("Please upload your location history zip for us to be able to display it on the map");
            }

            MapDetails map = new MapDetails
            {
                MinLong         = double.Parse(leftLong),
                MaxLong         = double.Parse(rightLong),
                MaxLat          = double.Parse(topLat),
                MinLat          = double.Parse(bottomLat),
                CenterLongitude = double.Parse(centerLong),
                CenterLatitude  = double.Parse(centerLat),
                ZoomLevel       =    int.Parse(zoom),
            };
                                  
            byte[] buffer = new byte[file.ContentLength];
            file.InputStream.Read(buffer, 0, file.ContentLength);

            string fileContentType = file.ContentType;
            string json;

            switch (fileContentType)
            {
                case "application/x-zip-compressed":
                    json = System.Text.Encoding.UTF8.GetString(Zippy.Unzip(buffer));
                    break;
                case "application/octet-stream":
                    json = System.Text.Encoding.UTF8.GetString(buffer);
                    break;
                default:
                    return Error("Unsupported file type. Only .zip and .json are supported");
            }

            try
            {
                using (Image outputImage = JsonParser.Parse(json, map))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        outputImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return File(ms.ToArray(), "image/jpeg");
                    }
                }
            }
            catch (ImageProcessingException ex)
            {
                return Error(ex.Message);
            }
            catch (Exception)
            {
                return Error("Failed to process image. Sorry");
            }
        }              
        
        public ActionResult Error(string error)
        {
            ViewBag.Error = error;

            return Index();
        }
    }
}