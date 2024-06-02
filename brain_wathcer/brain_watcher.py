import asyncio
import json

import numpy as np
import pandas as pd
import streamlit as st

import pymongo
# from pymongo import MongoClient

import matplotlib.pyplot as plt

import neurofeedback
import requests

from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi


def normalize_power(alpha_power, beta_power, theta_power):
    """
    Normalize the power values to a scale of 0 to 1.
    
    :param alpha_power: Power of alpha band
    :param beta_power: Power of beta band
    :param theta_power: Power of theta band
    :return: Normalized values of alpha, beta, and theta power
    """
    total_power = alpha_power + beta_power + theta_power
    alpha_norm = alpha_power / total_power
    beta_norm = beta_power / total_power
    theta_norm = theta_power / total_power
    
    return alpha_norm, beta_norm, theta_norm
    
def calculate_stress_level(alpha_norm, beta_norm, theta_norm):
    """
    Calculate the stress level as a value between 0 and 1.
    
    :param alpha_norm: Normalized alpha power
    :param beta_norm: Normalized beta power
    :param theta_norm: Normalized theta power
    :return: Stress level as a float between 0 and 1
    """
    # Weighted stress level calculation
    stress_level = (beta_norm - alpha_norm)/100 # Simplified formula for illustrative purposes
    stress_level = np.clip(stress_level, 0, 1)  # Ensure value is between 0 and 1
    
    return stress_level


def write_stress_level_to_file(stress_level, filename):
    """
    Write the normalized stress level to a file, appending it as a new line.
    
    :param stress_level: Normalized stress level
    :param filename: Name of the file to write to
    """
    with open(filename, 'a') as file:
        file.write(f"{stress_level}\n")
        
def write_stress_level_to_mongodb(stress_level):
    """
    Write the normalized stress level to MongoDB.
    
    :param stress_level: Normalized stress level
    """
    
    uri = "mongodb+srv://new_user_31:sC07g2ieVzJNNwyS@cluster0.efxqvcx.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0"
    # Create a new client and connect to the server
    client = MongoClient(uri)
    # Send a ping to confirm a successful connection
    
    # Connect to MongoDB
    db = client['wompwomp']  # Replace 'mydatabase' with your database name
    collection = db['stress_level']  # Replace 'stress_levels' with your collection name

    try:
        # Insert document with stress level
        collection.insert_one({"stress_level": stress_level})
    
    except Exception as e:
        print(e)


st.set_page_config(page_title="Brain Watcher", page_icon="📈")

st.markdown("# Brain Watcher")
st.write("""Here we will plot your brain activity in the line plot""")



st.divider()



col1, col2 = st.columns(2)
df_eeg = pd.DataFrame(columns=["alpha_metric", "beta_metric", "theta_metric"])

if col1.button("Start Wathcing your brain activity"):
    last_rows = np.random.randn(1, 3)

    chart = st.line_chart(last_rows)
    while True:
        alpha_metric, beta_metric, theta_metric = next(neurofeedback.neurofeedback_fn())
        new_rows = np.array([[alpha_metric, beta_metric, theta_metric]])
        chart.add_rows(new_rows)
        df_eeg = pd.concat([df_eeg, pd.DataFrame([{
            "alpha_relaxation": alpha_metric,
            "beta_concentration": beta_metric,
            "theta_focus": theta_metric
        }])], ignore_index=True)
        stress = calculate_stress_level(alpha_metric, beta_metric, theta_metric)
 
       
        write_stress_level_to_file(stress,'/Users/yzheng/Projects/ethprague/stress.txt')
        write_stress_level_to_mongodb(stress)

st.divider()

st.write(
    "The 0 lines plot your Alpha Relaxation value (Physically and Mentally Relaxed)")

st.write(
    "The 1 lines plot your Beta Concentration value (Awake, alert consciousness, thinking, excitement)"
)

st.write(
    "The 2 lines plot your Theta Relaxation value (Creativity, insight, deep focused states, reduced consciousness) "
)


