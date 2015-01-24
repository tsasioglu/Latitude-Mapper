using System;
namespace GoogleMapping.Controllers
{
    public class ImageProcessingException : Exception
    {
        public ImageProcessingException(string error) : base(error)
        {

        }
    }
}