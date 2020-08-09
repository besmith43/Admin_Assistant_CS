import QtQuick 2.9
import QtQuick.Controls 2.1
import QtQuick.Dialogs 1.1
import AdminAssistant 1.0

Page {

    Column {
        spacing: 40
        width: parent.width

        Text {
            id: notes
            x: 10
            y:10
            width: parent.width - 50
            text: "This will delete the temp user of the provided name created during standard Windows imaging." 
            font.family: monoFont.name
            wrapMode: Text.WordWrap
        }

        Rectangle {
            id: tempnameRectangle
            x: 10
            y: notes.height

            width: parent.width

            Text {
                id: tempnameText
                x: tempnameRectangle.x
                y: tempnameRectangle.y
                text: "Temp User Name"
                font.family: monoFont.name
            }

            TextField {
                id: tempnameField
                x: tempnameRectangle.x
                y: tempnameText.y + 20
                width: tempnameRectangle.width - 50
                focus: true
                KeyNavigation.tab: okButton
            }
        }
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
        KeyNavigation.tab: tempnameField

        function activate() {
            if (tempnameField.text.length > 0) {
                processingDialog.open()
                var task = model.runScript(tempnameField.text)
                Net.await(task, function(result) {
                    resultLabel.text = result
                    processingDialog.close()
                })
            }
            else {
                fillsBlankDialog.open()
            }
        }

        onClicked: okButton.activate()
        Keys.onReturnPressed: okButton.activate() // Enter key
        Keys.onEnterPressed: okButton.activate() // Numpad enter key
    }

    MessageDialog {
        id: fillsBlankDialog
        title: "Error"
        text: "The input field need to be completed to continue"
        icon: StandardIcon.Warning
        standardButtons: StandardButton.Ok
    }

    DeleteTempUserModel {
        id: model
    }
}