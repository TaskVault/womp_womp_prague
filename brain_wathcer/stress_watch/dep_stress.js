async function main() {
  const StressLevel = await ethers.getContractFactory("StressLevel");
  const stressLevel = await StressLevel.deploy();

  await stressLevel.deployed();

  console.log("StressLevel contract deployed to:", stressLevel.address);
}

main()
  .then(() => process.exit(0))
  .catch((error) => {
    console.error(error);
    process.exit(1);
  });
