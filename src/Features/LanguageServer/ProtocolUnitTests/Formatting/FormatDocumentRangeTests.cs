﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Roslyn.Test.Utilities;
using Xunit;
using LSP = Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.LanguageServer.UnitTests.Formatting
{
    public class FormatDocumentRangeTests : AbstractLanguageServerProtocolTests
    {
        [Fact]
        public async Task TestFormatDocumentRangeAsync()
        {
            var markup =
@"class A
{
{|format:void|} M()
{
            int i = 1;
    }
}";
            var expected =
@"class A
{
    void M()
{
            int i = 1;
    }
}";
            using var workspace = CreateTestWorkspace(markup, out var locations);
            var rangeToFormat = locations["format"].Single();
            var documentText = await workspace.CurrentSolution.GetDocumentFromURI(rangeToFormat.Uri).GetTextAsync();

            var results = await RunFormatDocumentRangeAsync(workspace.CurrentSolution, rangeToFormat);
            var actualText = ApplyTextEdits(results, documentText);
            Assert.Equal(expected, actualText);
        }

        private static async Task<LSP.TextEdit[]> RunFormatDocumentRangeAsync(Solution solution, LSP.Location location)
            => await GetLanguageServer(solution).ExecuteRequestAsync<LSP.DocumentRangeFormattingParams, LSP.TextEdit[]>(LSP.Methods.TextDocumentRangeFormattingName,
                solution, CreateDocumentRangeFormattingParams(location), new LSP.ClientCapabilities(), false, CancellationToken.None);

        private static LSP.DocumentRangeFormattingParams CreateDocumentRangeFormattingParams(LSP.Location location)
            => new LSP.DocumentRangeFormattingParams()
            {
                Range = location.Range,
                TextDocument = CreateTextDocumentIdentifier(location.Uri),
                Options = new LSP.FormattingOptions()
                {
                    // TODO - Format should respect formatting options.
                }
            };
    }
}
