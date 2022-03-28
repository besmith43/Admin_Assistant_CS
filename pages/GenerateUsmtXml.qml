import QtQuick 2.9
import QtQuick.Controls 2.1
import QtQuick.Dialogs 1.1
import AdminAssistant 1.0

Page {
    Text {
        id: notes
        x: 10
        y: 10
        width: parent.width - 50
        text: "This will create the XML's for use with the USMT application. \n\nPlease wait for the exit message before continuing on. \n\nThis is required before moving onto the following steps and will create a folder called USMT at C:\\Temp\\"
        font.family: monoFont.name
        wrapMode: Text.WordWrap
    }

    Label {
        id: resultLabel
        x: 10
        y: parent.height - 60
    }

    Button {
        id: okButton
        x: parent.width - 90
        y: parent.height - 60
        text: "Ok"
        font.family: monoFont.name

        function activate() {
            processingDialog.open()
            var task = model.runScript()
            Net.await(task, function(result) {
                resultLabel.text = result
                processingDialog.close()
            })
        }

        onClicked: okButton.activate()
        Keys.onReturnPressed: okButton.activate() // Enter key
        Keys.onEnterPressed: okButton.activate() // Numpad enter key
    }

    MessageDialog {
        id: fillsBlankDialog
        title: "Error"
        text: "Both input fields need to be completed to continue"
        icon: StandardIcon.Warning
        standardButtons: StandardButton.Ok
    }

    GenerateUsmtXmlModel {
        id: model
    }
}