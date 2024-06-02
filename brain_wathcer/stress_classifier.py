import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report

from kaggle.api.kaggle_api_extended import KaggleApi


# Initialize Kaggle API
kaggle_api = KaggleApi()

kaggle_api.dataset_download_files("Eye_State_Classification_EEG",path="/kaggle.csv", unzip=True)
dataset = load_dataset(os.path.join("/", "dataset.csv"))
    
# Load the dataset
df = pd.read_csv("kaggle_.csv")  # Update path to the downloaded dataset

# Split features (X) and labels (y)
X = df.drop(columns=["alpha_beta_metrics"])  # Assuming 'label' column contains the target variable
y = df["mental_state"]

# Split data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Initialize and train the RandomForestClassifier
model = RandomForestClassifier(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

# Predict on the testing set
y_pred = model.predict(X_test)

# Evaluate the model
print(classification_report(y_test, y_pred))

# Predict for new data
new_data = open("stress.txt").read()  # Update with the path to the new data file
predicted_stress_level = model.predict(new_data)
print("Predicted stress level:", predicted_stress_level)
