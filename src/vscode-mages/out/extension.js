"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.activate = activate;
exports.deactivate = deactivate;
const vscode_1 = require("vscode");
const node_1 = require("vscode-languageclient/node");
let client;
function activate(context) {
    const serverAssembly = context.asAbsolutePath('server/Mages.LanguageServer.dll');
    const serverOptions = {
        run: { command: 'dotnet', args: [serverAssembly], transport: node_1.TransportKind.stdio },
        debug: {
            command: 'dotnet',
            args: [serverAssembly],
            transport: node_1.TransportKind.stdio,
        }
    };
    const clientOptions = {
        documentSelector: [{ scheme: 'file', language: 'mages' }],
        synchronize: {
            fileEvents: vscode_1.workspace.createFileSystemWatcher('**/*.{mages,swm}')
        }
    };
    client = new node_1.LanguageClient('magesLanguageServer', 'MAGES Language Server', serverOptions, clientOptions);
    client.start();
}
function deactivate() {
    if (!client) {
        return undefined;
    }
    return client.stop();
}
//# sourceMappingURL=extension.js.map