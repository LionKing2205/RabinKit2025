using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Dynamic;
using System.Reflection;

namespace RabinKit.BlockLib.Extensions;

public static class JsRuntimeExtensions
{
    public static async Task<JsObjectReferenceDynamic> Import<T>(
        this IJSRuntime jsRuntime,
        string pathFromWwwRoot)
    {
        var libraryName = typeof(T).Assembly.GetName().Name;
        var path = Path.Combine($"/_content/{libraryName}/", pathFromWwwRoot);

        var module = await jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            path);
        return new JsObjectReferenceDynamic(module);
    }

    public class JsObjectReferenceDynamic : DynamicObject
    {
        public IJSObjectReference Module { get; }

        public JsObjectReferenceDynamic(IJSObjectReference module)
        {
            Module = module;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            var csharpBinder = binder.GetType().GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");
            var typeArgs =
                csharpBinder!.GetProperty("TypeArguments")?.GetValue(binder, null) as IList<Type> ??
                Array.Empty<Type>();

            var jsObjectReferenceType = typeof(IJSObjectReference);

            MethodInfo methodInfo;

            var method = jsObjectReferenceType
                .GetMethods()
                .First(x => x.Name.Contains(nameof(Module.InvokeAsync)));

            if (typeArgs.Any())
            {
                // only support one generic
                methodInfo = method.MakeGenericMethod(typeArgs.First());
            }
            else
            {
                methodInfo = method.MakeGenericMethod(typeof(IJSVoidResult));
            }

            var task = methodInfo.Invoke(Module, new object[] { binder.Name, args });
            result = task;
            return true;
        }
    }

}