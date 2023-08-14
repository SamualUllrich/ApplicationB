# ApplicationB

ApplicationB is a MAUI app designed to connect to ApplicationA and read data streamed via websockets. This connection enables you to receive continuous data streams, such as positions or sensor data, from ApplicationA.

## **Features**

* **Connect to ApplicationA:** ApplicationB connects to ApplicationA's REST API endpoint and opens a websocket client endpoint to read data.

* **Start and Stop Streaming:** Users can start and stop the data streaming process at any time.
* **Display Data:** The app provides a real-time view of the streamed data.

## **Getting Started**
### **Prerequisites**

* Visual Studio with .NET MAUI support
* A working instance of ApplicationA
* Appropriate emulator or physical device for testing

## **How to Use**
* Click the "Connect" button to start streaming data from ApplicationA.
* Click the "Disconnect" button to terminate the streaming.

### **Troubleshooting**
* Ensure that ApplicationA is running and accessible at the specified URL.
* Check the debug output for status messages and any potential errors.
* Make sure your emulator or device has proper network access.

### **Contributing**
For information on contributing to this project, please refer to the project's contributing guidelines.


