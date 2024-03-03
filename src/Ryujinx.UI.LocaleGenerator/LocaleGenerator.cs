using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Ryujinx.UI.LocaleGenerator
{
    [Generator]
    public class LocaleGenerator : IIncrementalGenerator
    {
        // 初始化方法，在此方法中注册生成源码的逻辑
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 获取所有以 "en_US.json" 结尾的附加文本文件
            var englishLocaleFile = context.AdditionalTextsProvider.Where(static x => x.Path.EndsWith("en_US.json"));

            // 从文本文件中提取内容的增量值提供程序
            IncrementalValuesProvider<string> contents = englishLocaleFile.Select((text, cancellationToken) => text.GetText(cancellationToken)!.ToString());

            // 注册源码输出方法
            context.RegisterSourceOutput(contents, (spc, content) =>
            {
                // 按行拆分内容，并选择以双引号开头的行，然后提取键名并组成枚举项
                var lines = content.Split('\n').Where(x => x.Trim().StartsWith("\"")).Select(x => x.Split(':')[0].Trim().Replace("\"", ""));
                StringBuilder enumSourceBuilder = new();
                enumSourceBuilder.AppendLine("namespace Ryujinx.Ava.Common.Locale;");
                enumSourceBuilder.AppendLine("internal enum LocaleKeys");
                enumSourceBuilder.AppendLine("{");
                foreach (var line in lines)
                {
                    enumSourceBuilder.AppendLine($"    {line},");
                }
                enumSourceBuilder.AppendLine("}");

                // 向生成的源代码容器中添加生成的枚举类型源码
                spc.AddSource("LocaleKeys", enumSourceBuilder.ToString());
            });
        }
    }
}

// 这段代码是一个 .NET 代码生成器，使用了 Roslyn 工具包。
// 它通过读取英文本地化文件的内容，提取键名，并以这些键名生成一个内部枚举类型。
// 生成的枚举类型用于在程序中表示文本本地化键，以便在编译时进行类型检查。