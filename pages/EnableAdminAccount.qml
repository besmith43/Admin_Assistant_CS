import QtQuick 2.9
import QtQuick.Controls 2.3
import QtQuick.Dialogs 1.1
import AdminAssistant 1.0

Page {
    Text {
        id: notes
        x: 10
        y: 10
        width: parent.width - 50
        text: "Please note that this action will result in the following addition actions: \n- Time Zone will be set to Central \n- Time Zone will be set to automatic \n- .Net Framework 3.5 will be installed"
        font.family: monoFont.name
        wrapMode: Text.WordWrap
    }

    Rectangle {
        id: adminPasswordRectangle
        x:10
        y: notes.height - 20
        width: parent.width

        Text {
            id: adminPasswordText
            x: adminPasswordRectangle.x
            y: adminPasswordRectangle.y
            text: "Local Administrator Password"
            font.family: monoFont.name
        }

        TextField {
            id: adminPasswordField
            x: adminPasswordRectangle.x
            y: adminPasswordRectangle.y + 20
            width: adminPasswordRectangle.width - 50
            focus: true
            KeyNavigation.tab: hostnameField
            echoMode: TextInput.Password
        }
    }

    Rectangle {
        id: hostnameRectangle
        x: 10
        y: adminPasswordField.y + 20
        width: parent.width

        Text {
            id: hostnameText
            x: hostnameRectangle.x
            y: hostnameRectangle.y
            text: "New Computer Name"
            font.family: monoFont.name
        }

        TextField {
            id: hostnameField
            x: hostnameRectangle.x
            y: hostnameRectangle.y + 20
            width: hostnameRectangle.width - 50
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
        KeyNavigation.tab: adminPasswordField

        function activate() {
            if (adminPasswordField.text.length > 0 && hostnameField.text.length > 0) {
                processingDialog.open()
                var task = model.runScript(adminPasswordField.text, hostnameField.text)
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
        text: "Both input fields need to be completed to continue"
        icon: StandardIcon.Warning
        standardButtons: StandardButton.Ok
    }

    EnableAdminAccountModel {
        id: model
    }
}