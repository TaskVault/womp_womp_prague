const { AVS } = require("eigenlayer-avs-sdk");

async function interactWithContract() {
  // Initialize AVS with your credentials
  const avs = new AVS({
    apiKey: env.AVS_API_KEY,
    apiSecret: env.AVS_API_SECRET,
    endpoint: "https://avs.eigenlayer.io/api/v1", // AVS API endpoint
  });

  // Contract address deployed in previous step
  const contractAddress = env.AVS_CONTRACT_ADDRESS;

  // Get contract instance
  const contract = avs.getContract("YourContract", contractAddress);

  // Call contract function
  const result = await contract.call("yourFunction", args);

  console.log("Result:", result);
}

interactWithContract().catch(console.error);
