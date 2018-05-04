﻿using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class RssController : Controller
    {
        /// <summary>
        /// A4 - XML External Entities (XXE) - This allows an external XML entity to be read and parsed as its default
        /// behavior is set to parse, allowing arbitrary fishing of information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoadFeed()
        {

            // Example of XML which is vulnerable.
            string xml = "<?xml version=\"1.0\" encoding=\"ISO - 8859 - 1\"?>" +
                "<!DOCTYPE foo [" +
                "<!ELEMENT foo ANY >" +
                "<!ENTITY xxe SYSTEM \"file:///C:/users/bdinger/documents/test.txt\" >]><foo>&xxe;</foo>";

            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
            xmlReader.DtdProcessing = DtdProcessing.Parse;
            var result = string.Empty;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    result = xmlReader.ReadElementContentAsString();
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// A4 - External XML Entities (XXE) - This will throw an exception when it parses the attempted attack 
        /// XML as the DtdProcessing has been set to Prohibit.
        /// </summary>
        /// <returns></returns>
        public ActionResult Feed()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"ISO - 8859 - 1\"?>" +
                         "<!DOCTYPE foo [" +
                         "<!ELEMENT foo ANY >" +
                         "<!ENTITY xxe SYSTEM \"file:///C:/users/bdinger/documents/test.txt\" >]><foo>&xxe;</foo>";

            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
            xmlReader.DtdProcessing = DtdProcessing.Prohibit;
            var result = string.Empty;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    result = xmlReader.ReadElementContentAsString();
                }
            }

            return Ok(result);
        }
    }
}
