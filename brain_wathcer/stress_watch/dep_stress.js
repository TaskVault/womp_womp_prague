const { ethers } = require("hardhat");

async function main() {
  const [deployer] = await ethers.getSigners();
  console.log("Deploying contracts with the account:", deployer.address);

  const StressLevel = await ethers.getContractFactory("StressLevel");
  const stressLevel = await StressLevel.deploy();

  console.log("StressLevel contract address:", stressLevel.address);
}

main()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });
