﻿using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Web;

namespace Sitecore.Support.Pipelines.GetChromeData
{
    /// <summary>
    /// The get field chrome data processor.
    /// </summary>
    public class GetFieldChromeData : GetChromeDataProcessor
    {
        /// <summary>
        /// The chrome type.
        /// </summary>
        public const string ChromeType = "field";

        /// <summary>
        /// The key of the Field instance in CustomData dictionary.
        /// </summary>
        public const string FieldKey = "field";

        /// <summary>
        ///  Path to the root item containing common web edit buttons
        /// </summary>
        private const string CommonButtonsRoot = "/sitecore/content/Applications/WebEdit/Common field buttons";

        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The pipeline args.</param>
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if (!"field".Equals(args.ChromeType, System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            Field field = args.CustomData["field"] as Field;
            Assert.ArgumentNotNull(field, "CustomData[\"{0}\"]".FormatWith(new object[]
			{
				"field"
			}));
            args.ChromeData.DisplayName = HttpUtility.HtmlEncode(field.DisplayName);
            if (!string.IsNullOrEmpty(field.ToolTip))
            {
                // Sitecore.Support.118340
                args.ChromeData.ExpandedDisplayName = HttpUtility.HtmlEncode(field.ToolTip.Replace("\"", "'"));
            }
            System.Collections.Generic.List<WebEditButton> buttons = GetFieldChromeData.GetButtons(field);
            base.AddButtonsToChromeData(buttons, args);
            System.Collections.Generic.List<WebEditButton> buttons2 = this.GetButtons("/sitecore/content/Applications/WebEdit/Common field buttons");
            base.AddButtonsToChromeData(buttons2, args);
        }

        /// <summary>
        /// Gets the web edit buttons.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The web edit buttons.</returns>
        private static System.Collections.Generic.List<WebEditButton> GetButtons(Field field)
        {
            Assert.ArgumentNotNull(field, "field");
            System.Collections.Generic.List<WebEditButton> list = new System.Collections.Generic.List<WebEditButton>();
            CustomField field2 = FieldTypeManager.GetField(field);
            if (field2 != null)
            {
                list.AddRange(field2.GetWebEditButtons());
            }
            else
            {
                list.AddRange(CustomField.GetWebEditButtons(field));
            }
            return list;
        }
    }
}