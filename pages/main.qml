import QtQuick 2.9
import QtQuick.Layouts 1.3
import QtQuick.Controls 2.3
import QtQuick.Controls.Material 2.1

/*
    TTU color Palette can be found here
    https://www.tntech.edu/ocm/marketingtoolkit/color.php
*/

ApplicationWindow {
    id: window
    width: 400
    height: 550
    visible: true
    title: "Admin-Assistant"

    Material.theme: Material.Light
    Material.accent: '#4f2984'
    Material.primary: '#4f2984'

    FontLoader { id: monoFont; source: "../fonts/JetBrainsMono-Regular.ttf" }

    Shortcut {
        sequences: ["Esc", "Back"]
        enabled: stackView.depth > 1
        onActivated: {
            stackView.pop()
            listView.currentIndex = -1
        }
    }

    Shortcut {
        sequence: "Menu"
        onActivated: optionsMenu.open()
    }

    header: ToolBar {
        Material.foreground: "white"

        RowLayout {
            spacing: 20
            anchors.fill: parent

            ToolButton {
                icon.source: stackView.depth > 1 ? "../images/back.png" : "../images/drawer.png"
                onClicked: {
                    if (stackView.depth > 1) {
                        stackView.pop()
                        listView.currentIndex = -1
                    } else {
                        drawer.open()
                    }
                }
            }

            Label {
                id: titleLabel
                text: listView.currentItem ? listView.currentItem.text : "Gallery"
                font.pixelSize: 20
                font.family: monoFont.name
                elide: Label.ElideRight
                horizontalAlignment: Qt.AlignHCenter
                verticalAlignment: Qt.AlignVCenter
                Layout.fillWidth: true
            }

            ToolButton {
                icon.source: "../images/menu.png"
                onClicked: optionsMenu.open()

                Menu {
                    id: optionsMenu
                    x: parent.width - width
                    transformOrigin: Menu.TopRight

                    MenuItem {
                        text: "About"
                        font.family: monoFont.name
                        onTriggered: aboutDialog.open()
                    }
                }
            }
        }
    }

    Drawer {
        id: drawer
        width: Math.min(window.width, window.height) / 3 * 2
        height: window.height
        interactive: stackView.depth === 1

        ListView {
            id: listView

            focus: true
            currentIndex: -1
            anchors.fill: parent

            delegate: ItemDelegate {
                width: parent.width
                text: model.title
                highlighted: ListView.isCurrentItem
                onClicked: {
                    listView.currentIndex = index
                    stackView.push(model.source)
                    drawer.close()
                }
            }

            model: ListModel {
                ListElement { title: "Enable Admin Account"; source: "../pages/EnableAdminAccount.qml" }
                ListElement { title: "Delete Temp User"; source: "../pages/DeleteTempUser.qml" }
                ListElement { title: "Connect to Domain"; source: "../pages/ConnectToDomain.qml" }
                ListElement { title: "Generate NAC Exception"; source: "../pages/GenerateNacException.qml" }
                ListElement { title: "Generate USMT XML"; source: "../pages/GenerateUsmtXml.qml" }
                ListElement { title: "Backup User Profile"; source: "../pages/BackupUserProfile.qml" }
                ListElement { title: "Load User Profile"; source: "../pages/LoadUserProfile.qml" }
            }

            ScrollIndicator.vertical: ScrollIndicator { }
        }
    }

    StackView {
        id: stackView
        anchors.fill: parent

        initialItem: Pane {
            id: pane

            /* starting window content */

            Text {
                x: 10
                y: 10
                width: parent.width - 50
                text: "This is test material.  Eventually I want this to have the Tech Logo"
                wrapMode: Text.WordWrap
                font.family: monoFont.name
            }
        }
    }
    
    Dialog {
        id: aboutDialog
        modal: true
        focus: true
        title: "About"
        x: (window.width - width) / 2
        y: window.height / 6
        width: Math.min(window.width, window.height) / 3 * 2

        Label {
            width: aboutDialog.availableWidth
            text: "This tool is designed to quicking perform common tasks that would otherwise be time consuming."
            wrapMode: Label.Wrap
            font.pixelSize: 12
            font.family: monoFont.name
        }
    }

    Dialog {
        id: processingDialog
        modal: true
        focus: true
        title: "Processing"
        x: (window.width - width) / 2
        y: window.height / 6
        width: Math.min(window.width, window.height) / 3 * 2

        BusyIndicator {
            running: true
        }
    }
}