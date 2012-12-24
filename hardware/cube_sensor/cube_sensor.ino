int PHOTOCELL_PIN = A0; // Analog Pin 0

void setup(){
  Serial.begin(9600);
}

void loop(){
  // Read the value from the analog pin
  int photocellReading = analogRead(PHOTOCELL_PIN); 

  // Print the value to the serial port
  Serial.println(photocellReading);
  
  // Slow down the output for easier reading
  delay(100);
}

