const { AVS } = require("eigenlayer-avs-sdk");
const env = require("hardhat");

async function deployContract() {
  // Initialize AVS with your credentials
  const avs = new AVS({
    apiKey: env.AVS_API_KEY,
    apiSecret: env.AVS_API_SECRET,
    endpoint: "https://avs.eigenlayer.io/api/v1", // AVS API endpoint
  });

  // Compile your contract to get the bytecode
  const compiledContract = compileContract();

  // Deploy the contract
  const deploymentResult = await avs.deployContract({
    contractName: "YourContract",
    bytecode: compiledContract.bytecode,
    abi: compiledContract.abi,
    gasLimit: 1000000, // Adjust gas limit as needed
    gasPrice: 1000000000, // Adjust gas price as needed
  });

  console.log("Contract deployed at:", deploymentResult.contractAddress);
}

deployContract().catch(console.error);
