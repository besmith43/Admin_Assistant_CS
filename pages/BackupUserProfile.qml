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
        text: "Please wait for the exit message before continuing. \n\nRequires that the Generate XML is run first."
        font.family: monoFont.name
        wrapMode: Text.WordWrap
    }

    Rectangle {
        id: userProfileRectangle
        x:10
        y: notes.height
        width: parent.width

        Text {
            id: userProfileText
            x: userProfileRectangle.x
            y: userProfileRectangle.y
            text: "User Profile to Backup"
            font.family: monoFont.name
        }

        TextField {
            id: userProfileField
            x: userProfileRectangle.x
            y: userProfileRectangle.y + 20
            width: userProfileRectangle.width - 50
            focus: true
            KeyNavigation.tab: okButton
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
        KeyNavigation.tab: userProfileField

        function activate() {
            if (userProfileField.text.length > 0) {
                processingDialog.open()
                var task = model.runScript(userProfileField.text)
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

    BackupUserProfileModel {
        id: model
    }
}