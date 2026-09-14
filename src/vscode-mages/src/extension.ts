import { ExtensionContext, workspace } from 'vscode';
import {
    LanguageClient,
    LanguageClientOptions,
    ServerOptions,
    TransportKind,
} from 'vscode-languageclient/node';

let client: LanguageClient | undefined;

export function activate(context: ExtensionContext) {
    const serverAssembly = context.asAbsolutePath('server/Mages.LanguageServer.dll');

    const serverOptions: ServerOptions = {
        run: { command: 'dotnet', args: [serverAssembly], transport: TransportKind.stdio },
        debug: {
            command: 'dotnet',
            args: [serverAssembly],
            transport: TransportKind.stdio,
        }
    };

    const clientOptions: LanguageClientOptions = {
        documentSelector: [{ scheme: 'file', language: 'mages' }],
        synchronize: {
            fileEvents: workspace.createFileSystemWatcher('**/*.{mages,swm}')
        }
    };

    client = new LanguageClient(
        'magesLanguageServer',
        'MAGES Language Server',
        serverOptions,
        clientOptions
    );

    client.start();
}

export function deactivate(): Thenable<void> | undefined {
    if (!client) {
        return undefined;
    }
    return client.stop();
}
