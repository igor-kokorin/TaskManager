using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain;
using TaskManager.Web.Utilities;

namespace TaskManager.Web.TagHelpers
{
    public class TaskStatusSelectorTagHelper: TagHelper
    {
        public WorkItemStatus? CurrentValue { get; set; }
        public IEnumerable<WorkItemStatus> AllowedValues { get; set; }
        public WorkItemStatus? DefaultValue { get; set; } = WorkItemStatus.ASSIGNED;

        private string BuildButton(WorkItemStatus statusValue)
        {
            var classes = new List<string>() { "ui", "button" };

            var isCurrentValue = false;

            if (CurrentValue.HasValue)
            {
                if (CurrentValue == statusValue)
                {
                    classes.Add("active");
                    isCurrentValue = true;
                }
            }
            else
            {
                if (statusValue == DefaultValue)
                {
                    classes.Add("active");
                    isCurrentValue = true;
                }
            }

            if (AllowedValues != null && !AllowedValues.Contains(statusValue) && !isCurrentValue)
            {
                classes.Add("disabled");
            }

            var buttonTemplate = $"<button type=\"button\" data-status=\"{statusValue}\" class=\"{string.Join(" ", classes)}\">{statusValue.GetDisplayName()}</button>";

            return buttonTemplate;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var statusValues = Enum.GetValues(typeof(WorkItemStatus)).Cast<WorkItemStatus>();

            StringBuilder sb = new StringBuilder();

            sb.Append($"<input type=\"hidden\" id=\"Status\" name=\"Status\" value=\"{CurrentValue ?? DefaultValue}\">");

            sb.Append("<div class=\"four ui buttons status-toggle\">");

            foreach (var statusValue in statusValues)
            {
                sb.Append(BuildButton(statusValue));
            }

            sb.Append("</div>");

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}

