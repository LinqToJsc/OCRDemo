using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using FCEngine;
using TDF.Demo.AdminWeb.Models.OCR.Tools;

namespace TDF.Demo.AdminWeb.Models.OCR
{
    public static class OcrImgToDataServer
    {
        static IEngine engine;
        static IFlexiCaptureProcessor processor;
        static  Image processedImage;

        static Dictionary<string, object> tempDatas = new Dictionary<string, object>();

        public static Tuple<Dictionary<string, object>,List<object>>  GetData(string imgPath)
        {
            
            if (engine == null)
            {
                engine = loadEngine();
            }
            if (processor == null)
            {
                processor = engine.CreateFlexiCaptureProcessor();
                processor.AddDocumentDefinitionFile("C:\\ProgramData\\ABBYY\\SDK\\11\\FlexiCapture Engine\\Samples\\SampleMisc\\Invoice.fcdot");
            }
            else
            {
                processor.ResetProcessing();
            }
            processor.AddImageFile(imgPath);
            IDocument document = processor.RecognizeNextDocument();
            if (document != null && document.DocumentDefinition != null)
            {
               return  ReadDocData(document);
            }
            return null;
        }



        private static Tuple<Dictionary<string, object>, List<object>> ReadDocData(IDocument document)
        {
            //ImgDatas imgDatas = new ImgDatas();


            Dictionary<string,object> imgDatas = new Dictionary<string, object>();
            List<object> listData = new List<object>();


            IImageDocument pageImageDocument = document.Pages[0].ReadOnlyImage;
            IImage pageColorImage = pageImageDocument.ColorImage;
            IHandle hBitmap = pageColorImage.GetPicture(null, 0);
            processedImage = System.Drawing.Image.FromHbitmap(hBitmap.Handle);

            IField firstSection = document.Sections[0];
            addDocumentNodeChildren(imgDatas, listData, firstSection.Children);
            
            return new Tuple<Dictionary<string, object>, List<object>>(imgDatas, listData);
        }


        private static void addDocumentNode(Dictionary<string, object> imgDatas, List<object> listData, IField documentNode)
        {
            IFieldValue value = documentNode.Value;
            if (value != null)
            {
                var key = documentNode.Name;
                var val = value.AsString;
                imgDatas.Add(key, val);
            }
            if (documentNode.Instances != null)
            {
                addDocumentNodeInstances(tempDatas, listData, documentNode.Instances);
            }
            else if (documentNode.Children != null)
            {
                //addDocumentNodeChildren(treeNode, documentNode.Children);
            }

        }

        private static void addDocumentNodeChildren(Dictionary<string, object> imgDatas, List<object> listData, IFields children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                addDocumentNode(imgDatas, listData, children[i]);
            }
        }

        private static void addDocumentNodeInstances(Dictionary<string, object> tempData, List<object> listData,
            IFieldInstances instances)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                if (tempData.Count > 0)
                {
                    listData.Add(tempData);
                    tempData = new Dictionary<string, object>();
                }
                if (instances[i].Children != null)
                {
                    addDocumentNodeChildren(tempData, listData, instances[i].Children);
                }
            }
        }

        private static IEngine loadEngine()
        {
            IEngine engine = null;
            int hresult = InitializeEngine(FceConfig.GetDeveloperSN(), out engine);
            Marshal.ThrowExceptionForHR(hresult);
            return engine;
        }

        [DllImport(FceConfig.DllPath, CharSet = CharSet.Unicode), PreserveSig]
        internal static extern int InitializeEngine(String devSN, out IEngine engine);

    }


    public class ImgDatas
    {
        public object Invoice { get; set; }

        public List<object>   InvoiceTable { get; set; }

    }
}