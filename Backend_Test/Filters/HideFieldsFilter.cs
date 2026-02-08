using Backend_Test.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace Backend_Test.Filters
{
    public class HideFieldsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hideFieldsAttr = context.MethodInfo.GetCustomAttribute<HideFieldsAttribute>();

            if (hideFieldsAttr != null)
            {
                // ตรวจสอบว่าเป็น POST method และมี request body
                if (context.ApiDescription.HttpMethod?.ToUpper() == "POST" && operation.RequestBody != null)
                {
                    foreach (var content in operation.RequestBody.Content)
                    {
                        if (content.Value.Schema?.Reference != null)
                        {
                            // ถ้ามี reference ไปยัง schema อื่น
                            var schemaReference = content.Value.Schema.Reference.Id;

                            // สร้าง inline schema แทน
                            var inlineSchema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>()
                            };

                            // คัดลอก properties ทั้งหมด ยกเว้นที่ต้องการซ่อน
                            var originalSchema = context.SchemaRepository.Schemas[schemaReference];
                            foreach (var property in originalSchema.Properties)
                            {
                                if (!hideFieldsAttr.Fields.Contains(property.Key))
                                {
                                    inlineSchema.Properties[property.Key] = property.Value;
                                }
                            }

                            content.Value.Schema = inlineSchema;
                        }
                        else if (content.Value.Schema?.Properties != null)
                        {
                            // ถ้าเป็น inline schema อยู่แล้ว
                            foreach (var field in hideFieldsAttr.Fields)
                            {
                                content.Value.Schema.Properties.Remove(field);
                            }
                        }
                    }
                }
            }
        }
    }
}
