﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGetGallery.FunctionTests.Helpers;
using System;
using System.IO;
using System.Net;

namespace NuGetGallery.FunctionalTests.ODataTests.Statistics
{
    [TestClass]
    public class PackageStatsTests
    {
        /// <summary>
        ///     Double-checks whether expected fields exist in the packages feed.
        /// </summary>
        [TestMethod]
        public void PackageFeedStatsSanityTest()
        {
            WebRequest request = WebRequest.Create(UrlHelper.V2FeedRootUrl + @"stats/downloads/last6weeks/");
            // Get the response.          
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string responseText = sr.ReadToEnd();

            string firstPackage = responseText.Substring(responseText.IndexOf("{"), responseText.IndexOf("}") - responseText.IndexOf("{"));

            Assert.IsTrue(firstPackage.Contains(@"""PackageId"": """), "Expected PackageId field is missing.");
            Assert.IsTrue(firstPackage.Contains(@"""PackageVersion"": """), "Expected PackageVersion field is missing.");
            Assert.IsTrue(firstPackage.Contains(@"""Gallery"": """), "Expected Gallery field is missing.");
            Assert.IsTrue(firstPackage.Contains(@"""PackageTitle"": """), "Expected PackageTitle field is missing.");
            Assert.IsTrue(firstPackage.Contains(@"""PackageIconUrl"": """), "Expected PackageIconUrl field is missing.");
            Assert.IsTrue(firstPackage.Contains(@"""Downloads"": "), "Expected PackageIconUrl field is missing.");
        }

        /// <summary>
        ///     Verify copunt querystring parameter in the Packages feed.
        /// </summary>
        [TestMethod]
        public void PackageFeedCountParameterTest()
        {
            WebRequest request = WebRequest.Create(UrlHelper.V2FeedRootUrl + @"stats/downloads/last6weeks/");
            // Get the response.          
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string responseText = sr.ReadToEnd();
            string[] separators = new string[1] { "}," };
            int packageCount = responseText.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.IsTrue(packageCount == 500, "Expected feed to contain 500 packages. Actual count: " + packageCount);

            request = WebRequest.Create(UrlHelper.V2FeedRootUrl + @"stats/downloads/last6weeks?count=5");
            // Get the response.          
            response = request.GetResponse();
            sr = new StreamReader(response.GetResponseStream());
            responseText = sr.ReadToEnd();

            packageCount = responseText.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.IsTrue(packageCount == 5, "Expected feed to contain 5 packages. Actual count: " + packageCount);
        }
    }
}
